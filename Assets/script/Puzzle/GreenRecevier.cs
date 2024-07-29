using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenRecevier : MonoBehaviour
{
    public float power;
    public Behaviour[] Connnected;
    private Light light;
    private void Start()
    {
        light = GetComponent<Light>();
    }
    private void Update()
    {
        power -= Time.deltaTime;
        if(power < 0.0f )power = 0.0f;
        light.intensity = (power > 0.0f)? 1:0;
    }
}
