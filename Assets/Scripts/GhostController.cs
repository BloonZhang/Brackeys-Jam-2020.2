using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GhostController: Controls the ghosts that appear when Senko-san dies
public class GhostController : MonoBehaviour
{
    // static list that holds all ghosts
    public static List<GhostController> ghostList;

    // timer
    private float timeToExist = 10f;
    private float timer = 0f;

    void Awake()
    {
        if (ghostList == null) {ghostList = new List<GhostController>();}
        ghostList.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        // increment timer
        timer += Time.deltaTime;
        // If timer runs out, remove
        if (timer > timeToExist) 
        {
            ghostList.Remove(this); 
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        ghostList.Remove(this);
    }
}
