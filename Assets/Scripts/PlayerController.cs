using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //////// Singleton shenanigans ////////
    private static PlayerController _instance;
    public static PlayerController Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    // Use CharacterController2D from brackeys video, https://www.youtube.com/watch?v=dwcT-Dch0bA
    public CharacterController2D controller;

    // public prefabs and gameobjects
    public GameObject ghostPrefab;
    //public GameObject spawnPoint;

    // public variables
    public float runSpeed = 30f;

    // private variables
    public Vector3 defaultSpawnPoint;
    private Sprite originalSprite;

    // helper variables
    float horizontalMove = 0f;
    bool jump = false;

    // rules variables
    private bool reverseControls = false;
    private bool noLeft = false;
    private bool alwaysJump = false;
    private Vector3? customSpawnPoint = null;

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}
        // Set default variables
        defaultSpawnPoint = new Vector3(-5.13f, -0.21f, 0);
        originalSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        // Panic
        if (Input.GetButtonDown("Cancel")) {Death();}

        // Get inputs
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (reverseControls) {horizontalMove = -horizontalMove;}
        if (noLeft) {horizontalMove = Mathf.Max(0f, horizontalMove);}

        if (alwaysJump || Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        // Move. No crouch
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    // Enter trigger
    void OnTriggerEnter2D(Collider2D col)
    {
        // Spikes
        if (col.tag == "Spikes")
        {
            //Debug.Log("oof");
            Death();
        }
        // Lever
        else if (col.tag == "Lever")
        {
            //col.GetComponent<LeverController>().FlipSwitch();
            col.GetComponent<LeverController>().TurnOn();
        }
        // Meat
        else if (col.tag == "Meat")
        {
            col.GetComponent<MeatController>().Eat();
        }
    }

    // Rules methods
    public void ReverseControls() { reverseControls = true; }
    public void NoLeftControls() { noLeft = true; }
    public void AlwaysJump() { alwaysJump = true; }
    public void SetCustomSpawnPoint(Vector3 spawnPoint) { customSpawnPoint = spawnPoint; }
    public void SetCustomSprite(Sprite customSprite) { this.gameObject.GetComponent<SpriteRenderer>().sprite = customSprite; }
    public void ResetControls()
    { 
        reverseControls = false; 
        noLeft = false;
        alwaysJump = false;
        customSpawnPoint = null;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = originalSprite;
    }

    // Public methods
    public void Reset()
    {
        // Move player to spawn point
        /*
        controller.ResetFlip();
        PlayerController newFox = Instantiate(this, spawnPoint.transform.position, Quaternion.identity);
        newFox.gameObject.name = this.gameObject.name;
        Destroy(this.gameObject);
        */
        controller.ResetFlip();
        if (customSpawnPoint != null) {this.gameObject.transform.position = (Vector3)customSpawnPoint;}
        else {this.gameObject.transform.position = defaultSpawnPoint;}
    }

    // Helper methods
    // Create a ghost and die
    void Death()
    {
        //Debug.Log("dead");
        // Spawn ghost
        GameObject ghost = (GameObject) Instantiate(ghostPrefab, this.transform.position, Quaternion.identity);
        ghost.GetComponent<Rigidbody2D>().velocity = this.transform.GetComponent<Rigidbody2D>().velocity;
        ghost.GetComponent<SpriteRenderer>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;

        // Refresh the level, including doors and levers and player
        RulesManager.Instance.RefreshLevel();
    }
}
