using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Green : MonoBehaviour
{
    public float power = 0.0f;
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

    public void Pull()
    {
        Debug.Log("pulling !");
    }

    public void OnRetract()
    {
        Debug.Log("retract");
    }

    private void Update()
    {
        power -= Time.deltaTime;
        if(power < 0.0f) power = 0.0f;
    }
}
