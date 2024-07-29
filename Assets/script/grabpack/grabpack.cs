using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static grabpack;

public class grabpack : MonoBehaviour
{
   

    public Transform OriginLeft,OriginRight;
    public Transform LeftHand;
    public Transform[] RightHands;
    public GameObject[] RightHandsMesh;

    public int[] UnlockedHand;

    public Transform LeftGun, RightGun;
    public Transform LeftLineStart, RightLineStart;
    public float MaxDis,HandSpeed;
    public LineRenderer LeftLine,RightLine;
    public Transform NewLocationLeft,NewLocationRight;

    
    public Transform swing;
    public float SwingEffect;
    public int selected_hand;

    public AudioClip SoundFire,SoundRetract,SoundPull;
    private int hand_to_switch;

    private GrabGun left;
    private GrabGun right;
    public Animator anim;
    //animator for grabgun
    public Animator RightAnimator;
    public Animator LeftAnimator;
    // Start is called before the first frame update
    void Start()
    {
        hand_to_switch = -1;

        //ini left
        left = new GrabGun();
        left.hand_beavhiour = LeftHand.GetComponent<Hand>();
        left.origin = OriginLeft;
        left.hand = LeftHand;
        left.gun = LeftGun;
        left.line_start = LeftLineStart;
        left.line = LeftLine;
        left.button = 0;
        left.parent = this;

        //ini right
        right = new GrabGun();
        right.hand_beavhiour = RightHands[UnlockedHand[selected_hand]].GetComponent<Hand>();
        right.origin = OriginRight;
        right.hand = RightHands[UnlockedHand[selected_hand]];
        right.gun = RightGun;
        right.line_start = RightLineStart;
        right.line = RightLine;
        right.button = 1;
        right.parent = this;
        SetRightHandActive(selected_hand);


    }




    private void LateUpdate()
    {
        //call the tick update
        left.UpdateTick();
        right.UpdateTick();

        //check for hand switching
        if(hand_to_switch == -1)
        {
            //mouse wheel
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) hand_to_switch = selected_hand + 1;
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) hand_to_switch = selected_hand - 1 + UnlockedHand.Length;
            else { 
                for(int i=0; i < 9;i++) 
                {
                    if (Input.GetKeyDown((KeyCode)i + 49)) hand_to_switch = i;
                }
            }

            if(hand_to_switch != -1) 
            {
                if (right.grab)
                {
                    right.grab = false;
                }
                else
                {
                    hand_to_switch = (hand_to_switch + UnlockedHand.Length) % UnlockedHand.Length;
                    //switch hand
                    RightAnimator.SetTrigger("switch");
                    right.stop_look = true;
                }
            }
        }

        //make the launcher right position
        if (right.stop_look)
        {
            OriginRight.position = NewLocationRight.position;
            OriginRight.rotation = NewLocationRight.rotation;
        }
    }

    private void FixedUpdate()
    {
        //call the fixedtcik update
        left.FixedUpdateTick();
        right.FixedUpdateTick();

        //recuce rotation for swing eeffect not working
        //Debug.Log(swing.localEulerAngles);
        //swing.localEulerAngles = new Vector3(swing.localEulerAngles.x - 180 * SwingEffect + 180, 0, 0);
    }

    public void SwitchHand()
    {
        //set acivate hand
        selected_hand = hand_to_switch;
        SetRightHandActive(selected_hand);

        
    }

    public void ResetLook()
    {
        //rest the right.look
        right.stop_look = false;

        //reset hand to swtich
        hand_to_switch = -1;
    }

    public void SetRightHandActive(int index)
    {
        for (int i = 0; i < RightHandsMesh.Length; i++)
        {
            RightHandsMesh[i].SetActive(false);
        }

        for (int i =0; i < UnlockedHand.Length; i++)
        {
            RightHandsMesh[UnlockedHand[i]].SetActive(i == index);
            RightHands[UnlockedHand[i]].parent = OriginRight;
            if (i == index)
            {
                right.hand = RightHands[UnlockedHand[i]];
                right.hand_beavhiour = RightHands[UnlockedHand[i]].GetComponent<Hand>();
            }
        }

    }
    

    //grabgun class important thigs is here
    public class GrabGun : MonoBehaviour
    {
        public Transform origin;
        public Transform hand;
        public Transform gun;
        public Transform line_start;
        public int button = 0;
        public bool grab = false;
        public Vector3 point, norm;
        public LineRenderer line;
        public grabpack parent;
        public Hand hand_beavhiour;
        public bool stop_look = false;
        public Transform grab_object;
        public bool pull = false;
        private int last_retract;
        private int frame = 0;
        private int last_grab = 0;

        public void UpdateTick()
        {
            if (Input.GetMouseButtonDown(button))
            {
                if (grab)
                {
                    
                    grab = false;
                    if (grab_object.GetComponent<Rigidbody>())
                    {
                        pull = true;
                    }
                    else
                    {

                        //play the rectract sound
                        PLayAudio(parent.SoundRetract);
                    }
                }
                else
                {
                    Send("OnFire");
                    if (hand_beavhiour.CanFire)
                    {
                        //check the hand is at the launcher
                        Vector3 dif = hand.position - origin.position;
                        if (dif.magnitude < parent.HandSpeed)
                        {
                            RaycastHit hit;
                            if (Physics.Raycast(parent.transform.position, parent.transform.TransformDirection(Vector3.forward), out hit, parent.MaxDis))
                            {


                                //set parent and find the point
                                point = hit.point;
                                norm = hit.normal;
                                grab_object = hit.transform;


                            }
                            else
                            {
                                //set point in direction of the camera
                                point = parent.transform.position + parent.transform.TransformDirection(Vector3.forward) * parent.MaxDis;
                                grab_object = null;
                            }
                            hand.parent = null;
                            grab = true;

                            //play the sound 
                            PLayAudio(parent.SoundFire);
                        }
                    }

                }
            }

            if (Input.GetMouseButtonUp(button) && pull) 
            { 
                pull = false;
                PLayAudio(parent.SoundRetract);
            }

            //update puul sound
            if(pull && !origin.GetComponent<AudioSource>().isPlaying)
            {
                PLayAudio(parent.SoundPull);
            }

            //line
            Line();

            //gun point to the hand
            if(!stop_look) gun.LookAt(line.GetPosition(line.positionCount - 2),parent.transform.TransformDirection(Vector3.up));

           
        }
        public void FixedUpdateTick()
        {
            if (grab)
            {
                Vector3 dif = hand.position - point;
                if (dif.magnitude < parent.HandSpeed)
                {
                    //snap the hand to the point
                    hand.position = point;

                    //make hand child if rigidboddy
                    //if (grab_object GetComponent<Rigidbody>()) hand.parent = grab_object; 

                    //if object can't grab go back
                    if (! (hand_beavhiour.CanGrab && grab_object != null && (grab_object.GetComponent<Rigidbody>() != null || grab_object.GetComponent<Grabable>() != null || CheckSpecialGrab(grab_object))) )
                    {
                        grab = false;

                        //play audio
                        PLayAudio(parent.SoundRetract);
                    }

                    //LeftHand.rotation = Quaternion.LookRotation(norm,Vector3.up);
                    if (last_grab != frame - 1) hand_beavhiour.Script.BroadcastMessage("OnGrab",grab_object) ;

                    last_grab = frame;
                }
                else
                {
                    //move the hand to the point
                    hand.position -= dif.normalized * parent.HandSpeed;

                   
                }
            }
            else
            {
                if (pull)
                {
                    //pull object
                    hand.parent = grab_object;

                    Send("Pull");

                    grab_object.GetComponent<Rigidbody>();

                    if (line.positionCount < 3)
                    {
                        Vector3 dif = hand.position - origin.position;
                        if (dif.magnitude < parent.HandSpeed)
                        {

                        }
                        else
                        {
                            //move theobj to the launcher
                            AddGrabObjectForce( -dif.normalized * parent.HandSpeed);
                        }

                    }
                    else
                    {
                        Vector3 dif = hand.position - line.GetPosition(1);
                        if (dif.magnitude < parent.HandSpeed)
                        {

                            ShiftLeftLine();
                        }
                        else
                        {
                            //move the obj to the point
                            AddGrabObjectForce( - dif.normalized * parent.HandSpeed);
                        }
                    }
                }
                else
                {
                    hand.parent = null;
                    if (line.positionCount < 3)
                    {
                        Vector3 dif = hand.position - origin.position;
                        if (dif.magnitude < parent.HandSpeed)
                        {
                            //snap the hand to the launcher
                            hand.parent = origin;
                            hand.position = origin.position;
                            grab_object = null;
                            //reset vertex
                            line.positionCount = 2;

                            if (last_retract != frame - 1) Send("OnRetract");
                            last_retract = frame;
                        }
                        else
                        {
                            //move the hand to the launcher
                            hand.position -= dif.normalized * parent.HandSpeed;


                        }

                        hand.eulerAngles = origin.eulerAngles;
                    }
                    else
                    {
                        Vector3 dif = hand.position - line.GetPosition(1);
                        if (dif.magnitude < parent.HandSpeed)
                        {
                            //snap
                            hand.position = line.GetPosition(1);

                            ShiftLeftLine();
                        }
                        else
                        {
                            //move the hand to the point
                            hand.position -= dif.normalized * parent.HandSpeed;


                        }
                    }
                }
            }



            frame++;
        }

        void Line()
        {
            //set extremites position
            line.SetPosition(0, hand.position);
            line.SetPosition(line.positionCount - 1, line_start.position);

            //detect intersection
            LineIntersect();

            if (line.positionCount > 2)
            {
                //symplify line
                LineSimplify();
            }

        }
        void LineIntersect()
        {
            //cacul vector dif
            RaycastHit hit;
            Vector3 PointToTest = line.GetPosition(line.positionCount - 2);
            Vector3 dif = PointToTest - line_start.position;


            if (Physics.Raycast(line_start.position, dif, out hit, dif.magnitude - 0.005f))
            {
                if(hit.transform != grab_object)
                { 
                Debug.Log("new vertex create because of object : " + hit.transform.gameObject.ToString() + " at : " + hit.point.ToString());
                //increase the number of vertex
                line.positionCount++;

                //update pos
                line.SetPosition(line.positionCount - 2, hit.point);
                line.SetPosition(line.positionCount - 1, line_start.position);
                }
            }
        }
        void LineSimplify()
        {
            //cacul vector dif
            RaycastHit hit;
            Vector3 PointToTest = line.GetPosition(line.positionCount - 3);
            Vector3 dif = PointToTest - line_start.position;
            if (!Physics.Raycast(line_start.position, dif, out hit, dif.magnitude))
            {
                Debug.Log("remove vertex to sypmlify");
                //decrease the number of vertex
                line.positionCount--;

                //update pos
                line.SetPosition(line.positionCount - 1, line_start.position);
            }
        }
        void ShiftLeftLine()
        {

            for (int i = 0; i < line.positionCount - 1; i++)
            {
                line.SetPosition(i, line.GetPosition(i + 1));
            }
            line.positionCount--;
        }

        void PLayAudio(AudioClip clip)
        {
            origin.GetComponent<AudioSource>().clip = clip;
            origin.GetComponent<AudioSource>().Play();
        }

        void AddGrabObjectForce(Vector3 dir)
        {
            grab_object.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
        }

        void Send(string func)
        {
            if(hand_beavhiour.Script != null)
            {
                hand_beavhiour.Script.BroadcastMessage(func);
            }
        }

        bool CheckSpecialGrab(Transform obj)
        {
            for(int i=0; i< hand_beavhiour.SpecialGrab.Length; i++)
            {
                string comp = hand_beavhiour.SpecialGrab[i];
                if (obj.gameObject.tag == comp) return true; 
            }
            return false;
        }
    }

    
}
