using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

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

    // Update is called once per frame
    void Update()
    {
        // Get inputs
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

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
        if (col.tag == "Lever")
        {
            col.GetComponent<LeverController>().FlipSwitch();
        }
    }

    // Helper methods
    // Create a ghost and die
    void Death()
    {
        //Debug.Log("dead");
        // Spawn ghost
        GameObject ghost = (GameObject) Instantiate(ghostPrefab, this.transform.position, Quaternion.identity);
        ghost.GetComponent<Rigidbody2D>().velocity = this.transform.GetComponent<Rigidbody2D>().velocity;
        // Spawn new player and delete current one
        controller.ResetFlip();
        PlayerController newFox = Instantiate(this, spawnPoint.transform.position, Quaternion.identity);
        newFox.gameObject.name = this.gameObject.name;
        Destroy(this.gameObject);

        RulesManager.Instance.RefreshLevel();
    }
}
