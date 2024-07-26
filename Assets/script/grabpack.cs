using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class grabpack : MonoBehaviour
{
   

    public Transform OriginLeft,OriginRight;
    public Transform LeftHand,RightHand;
    public Transform LeftGun, RightGun;
    public Transform LeftLineStart, RightLineStart;
    public float MaxDis,HandSpeed;
    public LineRenderer LeftLine,RightLine;
    public Transform swing;
    public float SwingEffect;
    private GrabGun left;
    private GrabGun right;
    // Start is called before the first frame update
    void Start()
    {

        //ini left
        left = new GrabGun();
        left.origin = OriginLeft;
        left.hand = LeftHand;
        left.gun = LeftGun;
        left.line_start = LeftLineStart;
        left.line = LeftLine;
        left.button = 0;
        left.parent = this;

        //ini right
        right = new GrabGun();
        right.origin = OriginRight;
        right.hand = RightHand;
        right.gun = RightGun;
        right.line_start = RightLineStart;
        right.line = RightLine;
        right.button = 1;
        right.parent = this;

    }




    private void LateUpdate()
    {
        //call the tick update
        left.UpdateTick();
        right.UpdateTick();
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

        public void UpdateTick()
        {
            if (Input.GetMouseButtonDown(button))
            {
                if (grab)
                {
                    grab = false;
                }
                else
                {
                    RaycastHit hit;
                    if (Physics.Raycast(parent.transform.position, parent.transform.TransformDirection(Vector3.forward), out hit, parent.MaxDis))
                    {
                        grab = true;

                        //set parent and find the point
                        point = hit.point;
                        norm = hit.normal;
                        hand.parent = null;

                    }

                }
            }


            //line
            Line();

            //gun point to the hand
            gun.LookAt(line.GetPosition(line.positionCount - 2),parent.transform.TransformDirection(Vector3.up));
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
                    //LeftHand.rotation = Quaternion.LookRotation(norm,Vector3.up);
                }
                else
                {
                    //move the hand to the point
                    hand.position -= dif.normalized * parent.HandSpeed;
                }
            }
            else
            {
                if (line.positionCount < 3)
                {
                    Vector3 dif = hand.position - origin.position;
                    if (dif.magnitude < parent.HandSpeed)
                    {
                        //snap the hand to the launcher
                        hand.parent = origin;
                        hand.position = origin.position;
                        //reset vertex
                        line.positionCount = 2;
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
                Debug.Log("new vertex create because of object : " + hit.transform.gameObject.ToString() + " at : " + hit.point.ToString());
                //increase the number of vertex
                line.positionCount++;

                //update pos
                line.SetPosition(line.positionCount - 2, hit.point);
                line.SetPosition(line.positionCount - 1, line_start.position);
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
    }

}
