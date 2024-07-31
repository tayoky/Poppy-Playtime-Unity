using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    private Image crosshair_img;

    void Start()
    {
        crosshair_img = GetComponent<Image>();
    }


    void Update()
    {
        crosshair_img.fillAmount = 1.0f - (Hand.cam.GetComponent<grabpack>().cable_lenght / Hand.cam.GetComponent<grabpack>().MaxDis);
    }
}
