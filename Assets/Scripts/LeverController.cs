using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{

    // static list that holds all levers
    public static List<LeverController> leverList;

    // public variables
    public DoorScript[] myDoors;

    // private varibles
    private bool isOn = false;
    public Vector3 defaultSpawnPoint;
    private Sprite originalSprite;

    // Rules variables
    private bool disabled = false;
    private Vector3? customSpawnPoint = null;

    void Awake()
    {
        // Create list if needed and add to list
        if (leverList == null) {leverList = new List<LeverController>();}
        leverList.Add(this);
        // Set default variables
        defaultSpawnPoint = new Vector3(1.3f, -4.28f, 0);
        originalSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    void Start()
    {

    }

    void OnDestroy()
    {
        leverList.Remove(this);
    }

    // Rules methods
    public void Disable() {disabled = true;}
    public void Enable() {disabled = false;}
    public void SetCustomSpawnPoint(Vector3 spawnPoint) { customSpawnPoint = spawnPoint; }
    public void SetCustomSprite(Sprite customSprite) { this.gameObject.GetComponent<SpriteRenderer>().sprite = customSprite; }
    public void ResetControls() 
    {
        customSpawnPoint = null; 
        this.gameObject.GetComponent<SpriteRenderer>().sprite = originalSprite;
        Enable();
    }

    // public methods
    // Reset the switch
    public void Reset()
    {
        isOn = false;
        this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        // Respawn location
        if (customSpawnPoint != null) { this.gameObject.transform.position = (Vector3)customSpawnPoint; }
        else { this.gameObject.transform.position = defaultSpawnPoint; }
    }
    // Flip the switch
    public void FlipSwitch()
    {
        // No action if disabled
        if (disabled) {return;}
        // Flip
        if (!isOn) {TurnOn();}
        else {TurnOff();}
    }
    // Turn on the switch
    public void TurnOn()
    {
        // No action if disabled
        if (disabled) {return;}
        // Turn on
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
        // No action if disabled
        if (disabled) {return;}
        // Turn off
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
