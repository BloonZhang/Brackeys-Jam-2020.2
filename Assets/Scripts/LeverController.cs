﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    // public variables
    public DoorScript[] myDoors;

    // private varibles
    private bool isOn = false;

    void Awake()
    {

    }


    // public methods
    // Reset the switch
    public void Reset()
    {
        isOn = false;
        this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
    }
    // Flip the switch
    public void FlipSwitch()
    {
        if (!isOn) {TurnOn();}
        else {TurnOff();}
    }
    // Turn on the switch
    public void TurnOn()
    {
        if (!isOn)
        {
            isOn = true;
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            foreach (DoorScript door in myDoors) {door.OpenDoor();}
        }
    }
    // Turn off the switch
    public void TurnOff()
    {
        if (isOn)
        {
            isOn = false;
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            foreach (DoorScript door in myDoors) {door.CloseDoor();}
        }
    }
    // Is it on or off
    public bool IsOn()
    {
        return isOn;
    }




}
