using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    // static list that holds all doors
    public static List<DoorScript> doorList;

    // public variables
    //public bool isOpen = false;

    // private variables
    private Vector3 fullClose;
    private Vector3 fullOpen;
    private float moveIncrement = 0.008f;

    void Awake()
    {
        if (doorList == null) {doorList = new List<DoorScript>();}
        doorList.Add(this);
    }

    void Start()
    {
        fullClose = this.transform.position;
        fullOpen = fullClose + new Vector3(0, 2, 0);
    }

    void OnDestroy()
    {
        doorList.Remove(this);
    }

    // public methods
    public void Reset()
    {
        CloseDoor();
    }
    public void OpenDoor()
    {
        StopAllCoroutines();
        StartCoroutine(MoveUp());
    }
    public void CloseDoor()
    {
        StopAllCoroutines();
        StartCoroutine(MoveDown());
    }

    // helper methods
    private IEnumerator MoveUp()
    {
        // Keep it moving
        while (this.transform.position.y < fullOpen.y) 
        {
            this.transform.position += new Vector3(0, 0.05f, 0);
            yield return new WaitForSeconds(moveIncrement);
        }
        // Done moving
        this.transform.position = fullOpen;
        yield return null;
    }

    private IEnumerator MoveDown()
    {
        // Keep it moving
        while (this.transform.position.y > fullClose.y) 
        {
            this.transform.position -= new Vector3(0, 0.05f, 0);
            yield return new WaitForSeconds(moveIncrement);
        }
        // Done moving
        this.transform.position = fullClose;
        yield return null;
    }
}
