using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float time = 15.0f;
    private bool activate = false;

    private void Update()
    {
        time -= Time.deltaTime;
        
        //destraoy
        if(time < 0.0f ) Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(activate) GetComponent<Rigidbody>().isKinematic = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        activate = true;
    }
}
