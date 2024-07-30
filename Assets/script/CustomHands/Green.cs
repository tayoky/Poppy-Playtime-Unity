using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Green : MonoBehaviour
{
    private Light light;

    public float power = 0.0f;
    private bool last;
    private AudioSource Sound;
    public void OnGrab(Transform obj)
    {
        if(obj.GetComponent<GreenPower>()) power = obj.GetComponent<GreenPower>().PowerTime;
        if (obj.GetComponent<GreenRecevier>())
        {
            float trans = obj.GetComponent<GreenRecevier>().power ;
            obj.GetComponent<GreenRecevier>().power = power;
            power =trans;
        }
    }

    private void Start()
    {
        light = GetComponent<Light>();
        Sound = GetComponent<AudioSource>();
    }




    private void Update()
    {
        power -= Time.deltaTime;
        if(power < 0.0f) power = 0.0f;

        //set light
        light.enabled = power > 0.0f;


        if (power >0.0f != last)
        {
            if (power > 0.0f) Sound.Play();
            else Sound.Stop();
        }
        
        //updtae last
        last = power > 0.0f;
    }
}
