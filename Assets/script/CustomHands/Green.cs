using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Green : MonoBehaviour
{
    public void OnGrab()
    {
        Debug.Log("grab");
    }

    public void Pull()
    {
        Debug.Log("pulling !");
    }

    public void OnRetract()
    {
        Debug.Log("retract");
    }
}
