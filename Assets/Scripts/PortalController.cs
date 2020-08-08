using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PortalController : MonoBehaviour
{

    //////// Singleton shenanigans ////////
    private static PortalController _instance;
    public static PortalController Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    // portals
    public Tile bluePortal;
    public Tile orangePortal;
    public Tilemap portalTilemap;

    // Array for holding coordinates of all portal locations
    List<Vector3Int> bluePortalList = new List<Vector3Int>();
    List<Vector3Int> orangePortalList = new List<Vector3Int>();

    // helper variables
    private bool coolingDown = false;
    private float portalCooldown = 0.2f;
    private float timer = 0f;

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}

        // Set up blue portals
        bluePortalList.Add(new Vector3Int(-12, 0, 0));
        // Set up corresponding orange portals
        orangePortalList.Add(new Vector3Int(15, -9, 0));
    }

    void Start()
    {
        // for testing purposes
        //TurnOnPortal(0);
    }

    void Update()
    {
        // Timer to cool down
        if (coolingDown)
        {
            timer += Time.deltaTime;
            if (timer > portalCooldown)
            {
                timer = 0;
                coolingDown = false;
            }
        }
    }

    /*
    // When something enters the trigger
    void OnTriggerEnter2D(Collider2D col)
    {
        // Can't teleport if cooling down
        if (coolingDown) {return;}

        // Get position and send to teleport
        // Position of contact
        Vector3 contactPoint = col.gameObject.GetComponent<BoxCollider2D>().ClosestPoint(transform.position);
        Debug.Log("contact point: " + contactPoint);
        Vector3 destinationVector = Teleport(portalTilemap.WorldToCell(contactPoint));
        col.gameObject.transform.SetPositionAndRotation(destinationVector, Quaternion.identity);
    }
    */

    // When something collides with the portal
    void OnCollisionEnter2D(Collision2D col)
    {
        // can't teleport if cooling down
        if (coolingDown) {return;}

        // get position and send to teleport
        Vector3 contactPoint = col.contacts[0].point;
        Vector3 destinationVector = Teleport(portalTilemap.WorldToCell(contactPoint));
        col.gameObject.transform.SetPositionAndRotation(destinationVector, Quaternion.identity);
    }

    // public methods
    // Turn off all portals
    public void Reset()
    {
        // Remove all portals
        portalTilemap.ClearAllTiles();
    }
    // Turn on portal by index
    public void TurnOnPortal(int index)
    {
        portalTilemap.SetTile(bluePortalList[index], bluePortal);
        portalTilemap.SetTile(orangePortalList[index], orangePortal);
    }
    // funky teleport method. returns tilemap position to teleport to
    public Vector3 Teleport(Vector3Int originVector)
    {
        int index;
        Vector3 destinationVector;
        index = bluePortalList.FindIndex(originVector.Equals);
        // stepped in a blue portal
        if (index != -1) 
        {
            destinationVector = portalTilemap.GetCellCenterWorld(orangePortalList[index]);
        }
        // stepped in an orange portal
        else 
        {
            index = orangePortalList.FindIndex(originVector.Equals);
            destinationVector = portalTilemap.GetCellCenterWorld(bluePortalList[index]);
        }

        coolingDown = true;
        return destinationVector;
    }

}
