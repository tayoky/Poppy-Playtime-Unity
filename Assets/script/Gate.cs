using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public bool open;
    public bool UseY = true;
    public float OpenY;
    public float speed = 0.1f;
    private float CloseY;
    private float X,Z;

    private void Start()
    {
        CloseY = transform.localPosition.z;
        X = transform.localPosition.x;
        Z = transform.localPosition.y;
    }
    //todo z not y
    private void FixedUpdate()
    {
        if (open) transform.localPosition += Vector3.forward * speed;
        else transform.localPosition -= Vector3.forward * speed;

        if (transform.localPosition.z < CloseY) transform.localPosition = new Vector3(X, Z, CloseY);
        if(transform.localPosition.z > OpenY) transform.localPosition = new Vector3(X, Z, OpenY);
    }

    public void Activate()
    {
        open = true;
    }

    public void Desactivate()
    {
        open = false;
    }

}
