using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool CanFire = true;
    public bool CanGrab = true;
    public bool CanPull = true;
    public Behaviour[] SpecialGrab;//things thta only this hand can grab
    public Behaviour Script;//custom beavhiour for this hand

}
