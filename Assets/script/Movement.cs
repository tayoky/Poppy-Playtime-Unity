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
    public Transform swing;//for swing effect

    public AudioClip[] SoundJump;
    public AudioClip[] SoundWalk;
    public AudioClip[] SoundSprint;

    private float Height;
    private Rigidbody player;
    private Action action;
    private Vector2 direction;
    private float speed;
    private Animator grabpackP; //aniamtor for first person anim
    private Vector3 move;
    private AudioSource audio_player;
    private bool grounded;

    enum Action
    {
        Walk,
        Crounch,
        Sprint
    }

    private void Start()
    {
        grounded = false;
        player = GetComponent<Rigidbody>();
        grabpackP = cam.GetComponent<Animator>();
        Height = HeightMax;

        //get audioplayer
        audio_player = GetComponent<AudioSource>();

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
        
        
        move = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));

        //apply speed
        move *= speed;

        //aply direction
        move =  Quaternion.AngleAxis(direction.x, Vector3.up) * move;

        //don't applt y for gravity
        player.velocity = new Vector3 (move.x,player.velocity.y,move.z);


        grounded = false;
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10);
        float FloorDistance = hit.distance;
        
        if (FloorDistance < 1.1f)
        {
            grounded = true;
            grabpackP.SetBool("jump", false);
            //jump
            if (Input.GetKeyDown(Jump))
            {
                player.velocity = new Vector3(player.velocity.x, JumpForce, player.velocity.z);

                //stat jump anim
                grabpackP.SetBool("jump", true);

                //play jump sound
                PLayRandomClip(SoundJump);
            }
        }


     }

    private void Update()
    {
        //turn
        direction.x += Input.GetAxis("Mouse X");
        direction.y -= Input.GetAxis("Mouse Y");

        //swing
        if(swing != null)
        {
            swing.localEulerAngles += new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"),0);
        }

        //bound up and down
        if (direction.y > 90) direction.y = 90;
        if (direction.y < -90) direction.y = -90;


        cam.position = player.transform.position + Vector3.up * Height;
        cam.eulerAngles = new Vector3(direction.y, direction.x, 0);


        //�pdate aniamtion
        grabpackP.SetBool("walk", action == Action.Walk && move.magnitude > 0.1f);
        grabpackP.SetBool("sprint", action == Action.Sprint && move.magnitude > 0.1f);

        //update sound
        if (grounded && move.magnitude > 0.1f && !audio_player.isPlaying)
        {
            if (action == Action.Walk) PLayRandomClip(SoundWalk);
            else if (action == Action.Sprint) PLayRandomClip(SoundSprint);
        }

    }


    public void PLayRandomClip(AudioClip[] clips)
    {
        int index = Random.Range(0, clips.Length);
        PLayAudio(clips[index]);
    }

    public void PLayAudio(AudioClip clip)
    {
        //simple function to play any clip
        audio_player.clip = clip;
        audio_player.Play();
    }

}
