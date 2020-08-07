using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    //////// Singleton shenanigans ////////
    private static FlashlightController _instance;
    public static FlashlightController Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    // private variables
    private bool isOn = false;
    private GameObject flashlight;

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}

        TurnOff();
    }

    // Start is called before the first frame update
    void Start()
    {
        flashlight = this.gameObject.transform.Find("Flashlight").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            flashlight.transform.position = PlayerController.Instance.gameObject.transform.position;
        }
    }

    // public methods
    public void TurnOff() { this.gameObject.SetActive(false); isOn = false;}
    public void TurnOn() { this.gameObject.SetActive(true); isOn = true;}

}
