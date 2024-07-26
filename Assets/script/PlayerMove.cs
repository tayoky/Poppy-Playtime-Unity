using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //exterieur
    public Vector2 MouseSense;
    public float WalkSpeed,JumpForce;
    public float HeightMax,HeightSpeed;
    public GameObject camera;
    public grabpack grabpackP;

    //interieur
    private Rigidbody player;
    private float CamRot,LastVel ;
    private float Height;
    void Start()
    {
        grabpackP = camera.GetComponentInChildren<grabpack>();

        //recupe le rigidbody
        player = GetComponent<Rigidbody>();
        //lock curseur
        Cursor.lockState  = CursorLockMode.Locked;

        LastVel = -0.1f;

        GameObject Spawn;
        Spawn = GameObject.FindGameObjectWithTag("Spawn");
        if(Spawn != null)
        {
            transform.position = Spawn.transform.position;
            transform.rotation = Spawn.transform.rotation;
            Destroy(Spawn);
        }
    }
    void FixedUpdate()
    {

        //move
        Vector3 prev = player.transform.position;
        player.transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        Vector3 move = player.transform.position - prev;
        player.transform.position = prev;
        move *= WalkSpeed;
        move.y = player.velocity.y;
        player.velocity = move;

        //jump 
        //todo a amelioré plus tard
        if ((Input.GetAxis("Jump") == 1) && (player.velocity.y < 0.1) && (player.velocity.y >= 0) && (LastVel < 0)) player.AddForce( Vector3.up  * JumpForce,ForceMode.VelocityChange);
        if (player.velocity.y != 0) LastVel = player.velocity.y;

    }
    private void Update()
    {
        //rotation
        player.transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * MouseSense.y);


        CamRot += Input.GetAxis("Mouse Y") * MouseSense.x;

        if(CamRot < -90) CamRot = -90;
        if (CamRot > 90) CamRot = 90;

        //height
        Height += HeightSpeed * Time.deltaTime * -(Input.GetAxis("Crouch") - 0.5f);
        if (Height < 0) Height = 0;
        if (Height > HeightMax) Height = HeightMax;
    }
    private void LateUpdate()
    {
        //la cam au bonne endroit
        camera.transform.position = player.transform.position;
        camera.transform.eulerAngles = Vector3.zero;
        camera.transform.Translate(0, Height, 0);
        camera.transform.eulerAngles = new Vector3( CamRot,player.transform.eulerAngles.y,0 );
        
    }
}
