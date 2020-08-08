﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System; // for actions

public class RulesManager : MonoBehaviour
{
    //////// Singleton shenanigans ////////
    private static RulesManager _instance;
    public static RulesManager Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    // Gameobjects and prefabs
    public GameObject VictoryMessage;
    public Sprite playerSprite;
    public Sprite leverSprite;
    public Sprite meatSprite;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI stageText;

    // private variables
    private int stageNo = 0;
    private float timer = 0;
    private List<StageClass> stageList;

    // helper variables
    public bool stageTransitionLock = false;

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}

        // Set stages and rules and whatnot
        StageClass.stageList = new List<StageClass>();
        stageList = StageClass.stageList;
        // 1: No rules
        stageList.Add(new StageClass(0, "Arrow Keys to Move"));
        stageList[0].SetRules = delegate() { return; };
        stageList[0].ClearRules = delegate() { return; };
        // 2: backwards
        stageList.Add(new StageClass(1, "First Death?"));
        stageList[1].SetRules = delegate() { PlayerController.Instance.ReverseControls(); };
        stageList[1].ClearRules = delegate() { PlayerController.Instance.ResetControls(); };
    }

    void Start()
    {
        // Set up text and rules for first stage
        stageText.text = "Stage 1 - " + stageList[0].stageName;
        stageList[0].SetRules();
    }

    void FixedUpdate()
    {
        // increment using milliseconds
        timer += Time.fixedDeltaTime * 1000;
        // handle timer
        // Min: (int)/60000 Sec: (int)/1000 %60 Mil: %1000
        timerText.text = ((int)timer / 60000) + ":" + 
                        (((int)timer / 1000) % 60).ToString("D2") + ":" + 
                        ((int)timer % 1000).ToString("D3");
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

    public void NextStage()
    {
        stageTransitionLock = true;
        StartCoroutine(scrollMessage());
    }
    public void LoadNextStage()
    {
        // Remove all ghosts and refresh the stage
        RefreshLevel();
        if (GhostController.ghostList != null)
        {
            foreach (GhostController ghost in GhostController.ghostList) { Destroy(ghost.gameObject); }
        }

        // Reset the rules, increment the stage
        stageList[stageNo].ClearRules();
        ++stageNo;
        // If we're out of stages, you win the game!
        if (stageNo >= stageList.Count) 
        {
            // testing purposes, just go to stage 1
            stageNo = 0;
            Debug.Log("you win!");
            // TODO: 
            //winGame();
        }

        // Set new stage rules
        stageText.text = "Stage " + stageList[stageNo].stageNo + " - " + stageList[stageNo].stageName;
        stageList[stageNo].SetRules();


        /*
        switch(stageNo)
        {
            // Case for testing quickly
            case 2:
                PlayerController.Instance.MakeFoxAllergic();
                PlayerController.Instance.MakeSpikesTasty();
                break;
            
            // Backwards Controls
            case 2:
                PlayerController.Instance.ReverseControls();
                break;
            // Low gravity
            case 3:
                Physics2D.gravity = new Vector3(0, -1f, 0);
                break;
            // flashlight
            case 4:
                //FlashlightController.Instance.TurnOn();
                FlashlightController.DarknessOn();
                break;
            // Can't move left
            case 5:
                PlayerController.Instance.NoLeftControls();
                PortalController.Instance.TurnOnPortal(0);
                break;
            case 6:
                PlayerController.Instance.AlwaysJump();
                break;
            case 8:
                PlayerController.Instance.SetCustomSpawnPoint(LeverController.leverList[0].defaultSpawnPoint);
                PlayerController.Instance.SetCustomSprite(leverSprite);
                LeverController.leverList[0].SetCustomSpawnPoint(PlayerController.Instance.defaultSpawnPoint);
                LeverController.leverList[0].SetCustomSprite(playerSprite);
                PlayerController.Instance.Reset();
                LeverController.leverList[0].Reset();
                break;
            case 16:
                PlayerController.Instance.SetCustomSpawnPoint(MeatController.meatList[0].defaultSpawnPoint);
                PlayerController.Instance.SetCustomSprite(meatSprite);
                MeatController.meatList[0].SetCustomSpawnPoint(PlayerController.Instance.defaultSpawnPoint);
                MeatController.meatList[0].SetCustomSprite(playerSprite);
                PlayerController.Instance.Reset();
                MeatController.meatList[0].Reset();
                break;
            case 18:
                PlayerController.Instance.MakeFoxAllergic();
                PlayerController.Instance.MakeSpikesTasty();
                break;
            default:
                break;
        }
        */
    }

    // Helper methods
    /*
    private void ResetRules()
    {   
        // All resets
        PlayerController.Instance.ResetControls();
        Physics2D.gravity = new Vector3(0, -9.81f, 0);
        //FlashlightController.Instance.TurnOff();
        FlashlightController.Instance.LetThereBeLight();
        PortalController.Instance.Reset();
        LeverController.leverList[0].ResetControls();
        MeatController.meatList[0].ResetControls();
        PlayerController.Instance.Reset();
        LeverController.leverList[0].Reset();
        MeatController.meatList[0].Reset();

        
        switch(stageNo)
        {
            case 2:
                PlayerController.Instance.ResetControls();
                break;
            case 3:
                Physics2D.gravity = new Vector3(0, -9.81f, 0);
                break;
            case 4:
                //FlashlightController.Instance.TurnOff();
                FlashlightController.Instance.LetThereBeLight();
                break;
            case 5:
                PlayerController.Instance.ResetControls();
                PortalController.Instance.Reset();
            case 6:
                PlayerController.Instance.ResetControls();
                break;
            case 8:
                PlayerController.Instance.ResetControls();
                LeverController.leverList[0].ResetControls();
                PlayerController.Instance.Reset();
                LeverController.leverList[0].Reset();
                break;
            case 16:
                PlayerController.Instance.ResetControls();
                MeatController.meatList[0].ResetControls();
                PlayerController.Instance.Reset();
                MeatController.meatList[0].Reset();
                break;
            case 18:
                PlayerController.Instance.ResetControls();
                break;
            default:
                break;
        }
        
    }
    */

    // Coroutines
    private IEnumerator scrollMessage()
    {
        float timeInterval = 0.005f; // Interval of time between movements
        float moveInterval = 12f; // Distance moved in each interval
        float alicia = 1f; // the amount of time the message pauses in the middle
        // Scroll up
        while (VictoryMessage.gameObject.transform.localPosition.x <= 0)
        {
            VictoryMessage.gameObject.transform.localPosition += new Vector3(moveInterval, 0, 0);
            yield return new WaitForSeconds(timeInterval);
        }
        // Wait
        yield return new WaitForSeconds(alicia);
        // Scroll out
        while (VictoryMessage.gameObject.transform.localPosition.x <= 1302f)
        {
            VictoryMessage.gameObject.transform.localPosition += new Vector3(moveInterval, 0, 0);
            yield return new WaitForSeconds(timeInterval);
        }
        // Teleport back and end the stage
        VictoryMessage.gameObject.transform.localPosition = new Vector3(-1500f, VictoryMessage.gameObject.transform.localPosition.y, 0);
        stageTransitionLock = false;
        LoadNextStage();
        yield return null;
    }
    

}

// class for holding information about a stage
public class StageClass
{
    // static list holding all stages
    public static List<StageClass> stageList;

    // Constructors
    private StageClass() {} // Disable default
    public StageClass(int no, string name) 
    {
        stageName = name;
        stageNo = no + 1;
    }

    public Action SetRules {get; set;}
    public Action ClearRules {get; set;}


    // public variables
    public string stageName;
    public int stageNo;

    // TODO: background color and stage color
}