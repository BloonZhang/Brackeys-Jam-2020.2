using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FlashlightController: Controls the flashlight that appears in some levels
public class FlashlightController : MonoBehaviour
{
    //////// Singleton shenanigans ////////
    private static FlashlightController _instance;
    public static FlashlightController Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    // gameobjects
    public GameObject darkness;

    // private variables
    //private bool isOn = false;
    private bool isOn = true;

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}

        //TurnOff();
        TurnOn();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            this.transform.position = PlayerController.Instance.gameObject.transform.position;
        }
    }

    // public methods
    public void TurnOff() { this.gameObject.SetActive(false); isOn = false;}
    public void TurnOn() { this.gameObject.SetActive(true); isOn = true;}

    public void DarknessOn() { darkness.gameObject.SetActive(true); }
    public void LetThereBeLight() { darkness.gameObject.SetActive(false); }
}
