using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool CanFire = true;
    public bool CanGrab = true;
    public bool CanPull = true;
    public string[] SpecialGrab;//things thta only this hand can grab
    public Behaviour Script;//custom beavhiour for this hand
    public  static Transform player { get { return GameObject.FindGameObjectWithTag("Player").transform; } }
    public static Transform cam { get { return GameObject.FindGameObjectWithTag("MainCamera").transform; } }


}
