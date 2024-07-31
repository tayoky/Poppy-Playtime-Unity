using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPower : MonoBehaviour
{
    public float PowerTime = 10f;
    private float time;

    public void Activate()
    {
        PowerTime = time;
        GetComponent<Light>().enabled = true;
    }

    public void Desactivate()
    {
        time = PowerTime;
        PowerTime = 0.0f;

        //desactivate the light
        GetComponent<Light>().enabled = false;
    }
}
