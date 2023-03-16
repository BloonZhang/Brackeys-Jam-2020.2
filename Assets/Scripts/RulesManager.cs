using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System; // for actions
using UnityEngine.UI;
using UnityEngine.Tilemaps;

// RulesManager: Controls changing the rules of each stage
// Rules are set up in SetUpStages()
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

    // Recoloring
    public Image BackgroundColor;
    public Tilemap groundTilemap;
    public Tilemap spikesTilemap;
    public SpriteRenderer doorSpriteRenderer;
    public SpriteRenderer leverSpriteRenderer;

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
        SetUpStages();
    }

    void Start()
    {
        // Set up text and rules for first stage
        stageText.text = "Stage 1 - " + stageList[0].stageName;
        stageList[0].SetRules();
        Recolor(stageList[stageNo].backgroundColor, stageList[stageNo].tileColor, stageList[stageNo].doorColor);
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
            SceneController.Instance.winGame();
            return;
        }

        // Set new stage rules and recolor the map
        stageText.text = "Stage " + stageList[stageNo].stageNo + " - " + stageList[stageNo].stageName;
        stageList[stageNo].SetRules();
        Recolor(stageList[stageNo].backgroundColor, stageList[stageNo].tileColor, stageList[stageNo].doorColor);
    }

    // Helper methods
    // This is the method for setting up stages
    private void SetUpStages()
    {
        // Create stagelist
        StageClass.stageList = new List<StageClass>();
        stageList = StageClass.stageList;
        int tmpStageNumber = 0;
        // Each stage has five things to consider: number, name, setrules, clearrules, and colors
        // 1: No rules
        tmpStageNumber = 0;
        stageList.Add(new StageClass(tmpStageNumber, "Arrow Keys to Move"));
        stageList[tmpStageNumber].SetRules = delegate() { return; };
        stageList[tmpStageNumber].ClearRules = delegate() { return; };
        stageList[tmpStageNumber].backgroundColor = new Color(0.914f, 0.792f, 0.463f, 1);
        stageList[tmpStageNumber].tileColor = new Color(0.698f, 0.42f, 0.055f, 1);
        stageList[tmpStageNumber].doorColor = new Color(0.635f, 0.165f, 0.016f, 1);
        /*
        // 2: debug
        tmpStageNumber = 1;
        stageList.Add(new StageClass(tmpStageNumber, "You Are What You Eat"));
        stageList[tmpStageNumber].SetRules = delegate() 
        {  
            PlayerController.Instance.SetCustomSpawnPoint(MeatController.meatList[0].defaultSpawnPoint);
            PlayerController.Instance.SetCustomSprite(RulesManager.Instance.meatSprite);
            MeatController.meatList[0].SetCustomSpawnPoint(PlayerController.Instance.defaultSpawnPoint);
            MeatController.meatList[0].SetCustomSprite(RulesManager.Instance.playerSprite);
            PlayerController.Instance.Reset();
            MeatController.meatList[0].Reset();
        };
        stageList[tmpStageNumber].ClearRules = delegate() 
        {  
            PlayerController.Instance.ResetControls();
            MeatController.meatList[0].ResetControls();
            PlayerController.Instance.Reset();
            MeatController.meatList[0].Reset();
        };
        */
        /*
        // x: template
        tmpStageNumber = ~;
        stageList.Add(new StageClass(tmpStageNumber, "~"));
        stageList[tmpStageNumber].SetRules = delegate() {  };
        stageList[tmpStageNumber].ClearRules = delegate() {  };
        stageList[tmpStageNumber].backgroundColor = new Color();
        stageList[tmpStageNumber].tileColor = new Color();
        stageList[tmpStageNumber].doorColor = new Color();
        */
        
        
        // 2: backwards
        tmpStageNumber = 1;
        stageList.Add(new StageClass(tmpStageNumber, "First Death?"));
        stageList[tmpStageNumber].SetRules = delegate() { PlayerController.Instance.ReverseControls(); };
        stageList[tmpStageNumber].ClearRules = delegate() { PlayerController.Instance.ResetControls(); };
        stageList[tmpStageNumber].backgroundColor = new Color(0.945f, 0.78f, 0.886f, 1);
        stageList[tmpStageNumber].tileColor = new Color(0.941f, 0.953f, 0.627f, 1);
        stageList[tmpStageNumber].doorColor = new Color(1, 1, 1, 1);
        // 3: low gravity
        tmpStageNumber = 2;
        stageList.Add(new StageClass(tmpStageNumber, "On the Moon"));
        stageList[tmpStageNumber].SetRules = delegate() { Physics2D.gravity = new Vector3(0, -1f, 0); };
        stageList[tmpStageNumber].ClearRules = delegate() { Physics2D.gravity = new Vector3(0, -9.81f, 0); };
        stageList[tmpStageNumber].backgroundColor = new Color(17f/255f, 54f/255f, 207f/255f, 1);
        stageList[tmpStageNumber].tileColor = new Color(141f/255f, 154f/255f, 164f/255f, 1);
        stageList[tmpStageNumber].doorColor = new Color(1, 1, 1, 1);
        // 4: Flashlight
        tmpStageNumber = 3;
        stageList.Add(new StageClass(tmpStageNumber, "Near-sighted"));
        stageList[tmpStageNumber].SetRules = delegate() { FlashlightController.Instance.DarknessOn(); };
        stageList[tmpStageNumber].ClearRules = delegate() { FlashlightController.Instance.LetThereBeLight(); };
        // 5: Always jumping
        tmpStageNumber = 4;
        stageList.Add(new StageClass(tmpStageNumber, "As the Rain Came Down My Window"));
        stageList[tmpStageNumber].SetRules = delegate() { PlayerController.Instance.AlwaysJump(); };
        stageList[tmpStageNumber].ClearRules = delegate() { PlayerController.Instance.ResetControls(); };
        stageList[tmpStageNumber].backgroundColor = new Color(215f/255f, 180f/255f, 253f/255f, 1);
        stageList[tmpStageNumber].tileColor = new Color(138f/255f, 129f/255f, 250f/255f, 1);
        stageList[tmpStageNumber].doorColor = new Color(50f/255f, 28f/255f, 207f/255f, 1);
        // 6: Lever closes door
        tmpStageNumber = 5;
        stageList.Add(new StageClass(tmpStageNumber, "Press ESC to Reset"));
        stageList[tmpStageNumber].SetRules = delegate() { DoorScript.doorList[0].ReverseDoor(); };
        stageList[tmpStageNumber].ClearRules = delegate() { DoorScript.doorList[0].ResetControls(); };
        stageList[tmpStageNumber].backgroundColor = new Color(243f/255f, 242f/255f, 173f/255f, 1);
        stageList[tmpStageNumber].tileColor = new Color(166f/255f, 233f/255f, 89f/255f, 1);
        stageList[tmpStageNumber].doorColor = new Color(199f/255f, 214f/255f, 42f/255f, 1);
        // 7: Portals
        tmpStageNumber = 6;
        stageList.Add(new StageClass(tmpStageNumber, "Hidden Portals"));
        stageList[tmpStageNumber].SetRules = delegate() 
        {  
            PlayerController.Instance.NoLeftControls();
            PortalController.Instance.TurnOnPortal(0);
        };
        stageList[tmpStageNumber].ClearRules = delegate() 
        {  
            PlayerController.Instance.ResetControls();
            PortalController.Instance.Reset();
        };
        stageList[tmpStageNumber].backgroundColor = new Color(64f/255f, 234f/255f, 246f/255f, 1);
        stageList[tmpStageNumber].tileColor = new Color(1, 1, 1, 1);
        stageList[tmpStageNumber].doorColor = new Color(1, 1, 1, 1);
        // 8: Control lever instead of fox
        tmpStageNumber = 7;
        stageList.Add(new StageClass(tmpStageNumber, "Kimi no Na wa"));
        stageList[tmpStageNumber].SetRules = delegate() 
        {  
            PlayerController.Instance.SetCustomSpawnPoint(LeverController.leverList[0].defaultSpawnPoint);
            PlayerController.Instance.SetCustomSprite(RulesManager.Instance.leverSprite);
            LeverController.leverList[0].SetCustomSpawnPoint(PlayerController.Instance.defaultSpawnPoint);
            LeverController.leverList[0].SetCustomSprite(RulesManager.Instance.playerSprite);
            PlayerController.Instance.Reset();
            LeverController.leverList[0].Reset();
        };
        stageList[tmpStageNumber].ClearRules = delegate() 
        {  
            PlayerController.Instance.ResetControls();
            LeverController.leverList[0].ResetControls();
            PlayerController.Instance.Reset();
            LeverController.leverList[0].Reset();
        };
        stageList[tmpStageNumber].backgroundColor = new Color(64f/255f, 140f/255f, 255f/255f, 1);
        stageList[tmpStageNumber].tileColor = new Color(108f/255f, 219f/255f, 122f/255f, 1);
        // 9: Invisible meat
        tmpStageNumber = 8;
        stageList.Add(new StageClass(tmpStageNumber, "Test Your Memory"));
        stageList[tmpStageNumber].SetRules = delegate() { MeatController.meatList[0].HideMeat(); };
        stageList[tmpStageNumber].ClearRules = delegate() { MeatController.meatList[0].ResetControls(); };
        stageList[tmpStageNumber].backgroundColor = new Color(233f/255f, 155f/255f, 50f/255f, 1);
        stageList[tmpStageNumber].tileColor = new Color(43f/255f, 24f/255f, 24f/255f, 1);
        stageList[tmpStageNumber].doorColor = new Color(0, 0, 0, 1);
        // 10: Invisible meat pt 2
        tmpStageNumber = 9;
        stageList.Add(new StageClass(tmpStageNumber, "Hide and Seek"));
        stageList[tmpStageNumber].SetRules = delegate() { MeatController.meatList[0].HideMeat(new Vector3(-5.48f, 3.86f, 0)); };
        stageList[tmpStageNumber].ClearRules = delegate() { MeatController.meatList[0].ResetControls(); };
        stageList[tmpStageNumber].backgroundColor = new Color(233f/255f, 140f/255f, 153f/255f, 1);
        stageList[tmpStageNumber].tileColor = new Color(241f/255f, 212f/255f, 212f/255f, 1);
        // 11: Control meat
        tmpStageNumber = 10;
        stageList.Add(new StageClass(tmpStageNumber, "You Are What You Eat"));
        stageList[tmpStageNumber].SetRules = delegate() 
        {  
            PlayerController.Instance.SetCustomSpawnPoint(MeatController.meatList[0].defaultSpawnPoint);
            PlayerController.Instance.SetCustomSprite(RulesManager.Instance.meatSprite);
            MeatController.meatList[0].SetCustomSpawnPoint(PlayerController.Instance.defaultSpawnPoint);
            MeatController.meatList[0].SetCustomSprite(RulesManager.Instance.playerSprite);
            PlayerController.Instance.Reset();
            MeatController.meatList[0].Reset();
        };
        stageList[tmpStageNumber].ClearRules = delegate() 
        {  
            PlayerController.Instance.ResetControls();
            MeatController.meatList[0].ResetControls();
            PlayerController.Instance.Reset();
            MeatController.meatList[0].Reset();
        };
        stageList[tmpStageNumber].backgroundColor = new Color(255f/255f, 107f/255f, 100f/255f, 1);
        stageList[tmpStageNumber].tileColor = new Color(253f/255f, 255f/255f, 76f/255f, 1);


    }
    // This is the method for recoloring the background
    public void Recolor(Color backgroundColor, Color tileColor, Color doorColor)
    {
        BackgroundColor.color = backgroundColor;
        groundTilemap.color = tileColor;
        spikesTilemap.color = tileColor;
        doorSpriteRenderer.color = doorColor;
        leverSpriteRenderer.color = doorColor;
    }

    // Coroutines
    private IEnumerator scrollMessage()
    {
        // If game win
        if (stageNo == stageList.Count - 1)
        {
            Transform senkoText = VictoryMessage.transform.Find("SenkoMoreMeatText");
            senkoText.GetComponent<TextMeshProUGUI>().text = "And now Senko is full!";
        } 


        float timeInterval = 0.005f; // Interval of time between movements
        float moveInterval = 20f; // Distance moved in each interval
        float alicia = 1f; // the amount of time the message pauses in the middle
        // Scroll up
        while (VictoryMessage.gameObject.transform.localPosition.x <= 0)
        {
            VictoryMessage.gameObject.transform.localPosition += new Vector3(moveInterval, 0, 0);
            yield return new WaitForSeconds(timeInterval);
        }
        // Wait
        VictoryMessage.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(alicia);
        // Scroll out
        while (VictoryMessage.gameObject.transform.localPosition.x <= 1300f)
        {
            VictoryMessage.gameObject.transform.localPosition += new Vector3(moveInterval, 0, 0);
            yield return new WaitForSeconds(timeInterval);
        }
        // Teleport back and end the stage
        VictoryMessage.gameObject.transform.localPosition = new Vector3(-1090f, VictoryMessage.gameObject.transform.localPosition.y, 0);
        stageTransitionLock = false;
        LoadNextStage();
        yield return null;
    }
}

// StageClass: class that holds information about a stage
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
        backgroundColor = new Color32(0xC8, 0xC8, 0xC8, 0xFF);
        tileColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        doorColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
    }

    // public rules
    public Action SetRules {get; set;}
    public Action ClearRules {get; set;}

    // public variables
    public string stageName;
    public int stageNo;
    public Color backgroundColor;
    public Color tileColor;
    public Color doorColor;
}