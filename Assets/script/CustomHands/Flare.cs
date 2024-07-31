using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flare : Hand
{

    public GameObject Prefab;
    public float force;
    
    public void OnFire()
    {
        GameObject Proj = Instantiate(Prefab);
        Proj.transform.position = transform.position;
        Proj.GetComponent<Rigidbody>().AddForce( Hand.cam.rotation *  Vector3.forward * force);
    }
}
