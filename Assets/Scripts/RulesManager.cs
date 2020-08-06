using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesManager : MonoBehaviour
{
    //////// Singleton shenanigans ////////
    private static RulesManager _instance;
    public static RulesManager Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    // Gameobjects and prefabs
    public GameObject VictoryMessage;


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
        StartCoroutine(scrollMessage());
    }
    public void EndStage()
    {

    }

    // Helper methods
    private IEnumerator scrollMessage()
    {
        float timeInterval = 0.01f;
        float moveInterval = 0.2f;
        float waitTime = 1f;
        // Scroll up
        while (VictoryMessage.gameObject.transform.position.x <= 0)
        {
            VictoryMessage.gameObject.transform.position += new Vector3(moveInterval, 0, 0);
            yield return new WaitForSeconds(timeInterval);
        }
        // Wait
        yield return new WaitForSeconds(waitTime);
        // Scroll out
        while (VictoryMessage.gameObject.transform.position.x <= 16.5f)
        {
            VictoryMessage.gameObject.transform.position += new Vector3(moveInterval, 0, 0);
            yield return new WaitForSeconds(timeInterval);
        }
        // Teleport back and end the stage
        VictoryMessage.gameObject.transform.position = new Vector3(-13.3f, VictoryMessage.gameObject.transform.position.y, 0);
        EndStage();
        yield return null;
    }
    

}
