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
    public GameObject spawnPoint;

    // public variables
    public float runSpeed = 30f;


    // helper variables
    float horizontalMove = 0f;
    bool jump = false;

    // rules variables
    private bool reverseControls = false;
    private bool noLeft = false;

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}
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

        if (Input.GetButtonDown("Jump"))
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
    public void ResetControls()
    { 
        reverseControls = false; 
        noLeft = false;
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
        this.gameObject.transform.position = spawnPoint.transform.position;
    }

    // Helper methods
    // Create a ghost and die
    void Death()
    {
        //Debug.Log("dead");
        // Spawn ghost
        GameObject ghost = (GameObject) Instantiate(ghostPrefab, this.transform.position, Quaternion.identity);
        ghost.GetComponent<Rigidbody2D>().velocity = this.transform.GetComponent<Rigidbody2D>().velocity;

        // Refresh the level, including doors and levers and player
        RulesManager.Instance.RefreshLevel();
    }
}
