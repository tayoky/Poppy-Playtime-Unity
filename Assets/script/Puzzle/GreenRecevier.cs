using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenRecevier : MonoBehaviour
{
    public float power= 0.0f;
    public Behaviour[] Connected;
    private Light light;
    private bool last_power = true;
    private void Start()
    {
        light = GetComponent<Light>();
    }
    private void Update()
    {
        power -= Time.deltaTime;

        //light and block minus
        if(power < 0.0f )power = 0.0f;
        light.intensity = (power > 0.0f)? 1:0;

        if (last_power != power > 0.0f)
        {
            for (int i = 0; i < Connected.Length; i++)
            {
                if (power > 0.0f)Connected[i].BroadcastMessage("Activate");
                else Connected[i].BroadcastMessage("Desactivate");
            }
        }

        //update last
        last_power = power >0.0f;
    }
}
