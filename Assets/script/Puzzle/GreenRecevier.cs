using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenRecevier : MonoBehaviour
{
    public float power;
    public GameObject[] Connected;
    private Light light;
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
        
        for(int i = 0; i < Connected.Length; i++)
        {
            if (Connected[i].GetComponent<Gate>()) Connected[i].GetComponent<Gate>().open = power > 0.0f;
        }

    }
}
