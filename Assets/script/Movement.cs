using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform cam;
    public KeyCode Jump, Crounch,Sprint;
    public float WalkSpeed, SprintSpeed, CrounchSpeed;
    public float JumpForce;
    public float HeightMax, HeightMin,HeightSpeed;
    private float Height;
    private Rigidbody player;
    private Action action;
    private Vector2 direction;
    private float speed;

    enum Action
    {
        Walk,
        Crounch,
        Sprint
    }

    private void Start()
    {
        player = GetComponent<Rigidbody>();
        Height = HeightMax;

        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        //find action
        if (Input.GetKey(Crounch)) action = Action.Crounch;
        else if (Input.GetKey(Sprint)) action = Action.Sprint;
        else action = Action.Walk;

        //height
        if (action == Action.Crounch) Height -= HeightSpeed;
        else Height += HeightSpeed;

        //bound maxx and min
        if(Height > HeightMax) Height = HeightMax;
        else if (Height < HeightMin) Height = HeightMin;

        //move


        //find speed
        if (action == Action.Walk) speed = WalkSpeed;
        else if (action == Action.Sprint) speed = SprintSpeed;
        else if (action == Action.Crounch) speed = CrounchSpeed;



        Vector3 move = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));

        //apply speed
        move *= speed;

        //aply direction
        move =  Quaternion.AngleAxis(direction.x, Vector3.up) * move;

        //don't applt y for gravity
        player.velocity = new Vector3 (move.x,player.velocity.y,move.z);


        //jump
        if (Input.GetKeyDown(Jump))
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10);
            float FloorDistance = hit.distance;
            if(FloorDistance < 1.1f)
            {
                player.velocity = new Vector3(player.velocity.x, JumpForce, player.velocity.z);
            }
        }


     }

    private void Update()
    {
        //turn
        direction.x += Input.GetAxis("Mouse X");
        direction.y -= Input.GetAxis("Mouse Y");


        //bound up and down
        if (direction.y > 90) direction.y = 90;
        if (direction.y < -90) direction.y = -90;


        cam.position = player.transform.position + Vector3.up * Height;
        cam.eulerAngles = new Vector3(direction.y, direction.x, 0);
    }


}
