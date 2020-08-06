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

    // private variables
    private int stageNo = 1;

    // helper variables
    private bool stageTransitionLock = false;

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}
    }

    // public methods
    public void RefreshLevel()
    {
        // Refresh player
        PlayerController.Instance.Reset();

        // Don't refresh things if stage is transitioning to next stage
        if (stageTransitionLock) { return; }

        // Refresh doors and levers
        foreach(DoorScript door in DoorScript.doorList) {door.Reset();}
        foreach(LeverController lever in LeverController.leverList) {lever.Reset();}
        foreach(MeatController meat in MeatController.meatList) {meat.Reset();}

    }

    // TODO: finish level
    public void NextStage()
    {
        stageTransitionLock = true;
        StartCoroutine(scrollMessage());
    }
    public void LoadNextStage()
    {
        // Refresh and increment the stage
        RefreshLevel();
        ++stageNo;
        // Remove all ghosts
        foreach (GhostController ghost in GhostController.ghostList) { Destroy(ghost.gameObject); }

        switch(stageNo)
        {
            // Backwards Controls
            case 2:
                PlayerController.Instance.ReverseControls();
                break;
            case 3:
                PlayerController.Instance.ResetControls();
                break;
            default:
                break;
        }
    }

    // Helper methods
    private IEnumerator scrollMessage()
    {
        float timeInterval = 0.005f;
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
        stageTransitionLock = false;
        LoadNextStage();
        yield return null;
    }
    

}
