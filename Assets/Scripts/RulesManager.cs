using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesManager : MonoBehaviour
{
    //////// Singleton shenanigans ////////
    private static RulesManager _instance;
    public static RulesManager Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////


    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}
    }


    // public methods
    public void RefreshLevel()
    {
        // Refresh doors and levers and meats
        foreach(DoorScript door in DoorScript.doorList) {door.Reset();}
        foreach(LeverController lever in LeverController.leverList) {lever.Reset();}
        foreach(MeatController meat in MeatController.meatList) {meat.Reset();}
    }

    // TODO: finish level
    public void NextStage()
    {

    }

    // Helper methods
    private void scrollMessage()
    {

    }

}
