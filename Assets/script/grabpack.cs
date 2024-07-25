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
    public int button;
    private bool GrabLeft,LeftGrabAir,GrabRight,RightGrabAir;
    public float MaxDis,HandSpeed;
    private Vector3 point,norm;
    public LineRenderer line;
    public Vector2 Dec;
    // Start is called before the first frame update
    void Start()
    {
        Dec = Vector2.zero;
        GrabLeft = false;
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(button))
        {
            if(GrabLeft)
            {
                GrabLeft = false;
            }
            else
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,MaxDis))
                {
                    GrabLeft = true;

                    //set parent and find the point
                    point = hit.point;
                    norm = hit.normal;
                    LeftHand.parent = null;

                }

            }
        }


        //line
        Line();

        //gun point to the hand
        LeftGun.LookAt(line.GetPosition(line.positionCount - 2));

    }

    private void FixedUpdate()
    {
        if (GrabLeft)
        {
            Vector3 dif = LeftHand.position - point;
            if(dif.magnitude < HandSpeed)
            {
                //snap the hand to the point
                LeftHand.position = point;
                //LeftHand.rotation = Quaternion.LookRotation(norm,Vector3.up);
            }
            else
            {
                //move the hand to the point
                LeftHand.position -= dif.normalized * HandSpeed;
            }
        }
        else
        {
            if(line.positionCount < 3)
            {
                Vector3 dif = LeftHand.position - OriginLeft.position;
                if (dif.magnitude < HandSpeed)
                {
                    //snap the hand to the launcher
                    LeftHand.parent = OriginLeft;
                    LeftHand.position = OriginLeft.position;
                    //reset vertex
                    line.positionCount = 2;
                }
                else
                {
                    //move the hand to the launcher
                    LeftHand.position -= dif.normalized * HandSpeed;


                }

                LeftHand.eulerAngles = OriginLeft.eulerAngles;
            }
            else
            {
                Vector3 dif = LeftHand.position - line.GetPosition(1);
                if (dif.magnitude < HandSpeed)
                {
                    //snap
                    LeftHand.position = line.GetPosition(1);

                    ShiftLeftLine();
                }
                else
                {
                    //move the hand to the point
                    LeftHand.position -= dif.normalized * HandSpeed;


                }
            }
        }


        //smooth rot
        Dec *= 0.8f;
       
    }


    void Line()
    {
        //set extremites position
        line.SetPosition(0, LeftHand.position);
        line.SetPosition(line.positionCount - 1, LeftLineStart.position);

        //detect intersection
        LineIntersect();

        if(line.positionCount > 2)
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
        Vector3 dif = PointToTest - LeftLineStart.position;


        if (Physics.Raycast(LeftLineStart.position, dif, out hit, dif.magnitude - 0.005f))
        {
            Debug.Log("new vertex create because of object : " + hit.transform.gameObject.ToString() + " at : " + hit.point.ToString());
            //increase the number of vertex
            line.positionCount++;

            //update pos
            line.SetPosition(line.positionCount - 2, hit.point);
            line.SetPosition(line.positionCount - 1, LeftLineStart.position);
        }
    }

    void LineSimplify()
    {
        //cacul vector dif
        RaycastHit hit;
        Vector3 PointToTest = line.GetPosition(line.positionCount - 3);
        Vector3 dif = PointToTest - LeftLineStart.position;
        if(!Physics.Raycast(LeftLineStart.position,dif, out hit, dif.magnitude))
        {
            Debug.Log("remove vertex to sypmlify");
            //decrease the number of vertex
            line.positionCount--;

            //update pos
            line.SetPosition(line.positionCount - 1, LeftLineStart.position);
        }
    }


    void ShiftLeftLine()
    {
        
        for (int i=0;i<line.positionCount -1;i++)
        {
            line.SetPosition(i,line.GetPosition(i + 1));
        }
        line.positionCount--;
    }

}
