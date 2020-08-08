using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatController : MonoBehaviour
{

    // static list that holds all meats
    public static List<MeatController> meatList;

    // private variables
    public Vector3 defaultSpawnPoint;
    private Sprite originalSprite;

    // Rules variables
    private Vector3? customSpawnPoint = null;

    void Awake()
    {
        // Create meatlist if necessary and add meat
        if (meatList == null) {meatList = new List<MeatController>();}
        meatList.Add(this);
        // Set up defaults
        defaultSpawnPoint = new Vector3(4.5f, 2.715f, 0); 
        originalSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Rules methods
    public void SetCustomSpawnPoint(Vector3 spawnPoint) { customSpawnPoint = spawnPoint; }
    public void SetCustomSprite(Sprite customSprite) { this.gameObject.GetComponent<SpriteRenderer>().sprite = customSprite; }
    public void HideMeat() { this.gameObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask; }
    public void HideMeat(Vector3 hideLocation) 
    {
        this.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        this.transform.position = hideLocation;
        HideMeat();
    }
    public void ResetControls() 
    {
        customSpawnPoint = null; 
        this.gameObject.GetComponent<SpriteRenderer>().sprite = originalSprite;
        this.gameObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
        this.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    // public methods
    public void Reset()
    {
        /*
        // Spawn a copy and delete current one
        MeatController newMeat = Instantiate(this, spawnLocation, Quaternion.identity);
        newMeat.gameObject.name = this.gameObject.name;
        Destroy(this.gameObject);
        */
        this.gameObject.SetActive(true);
        // Respawn location
        if (customSpawnPoint != null) { this.gameObject.transform.position = (Vector3)customSpawnPoint; }
        else { this.gameObject.transform.position = defaultSpawnPoint; }
    }
    public void Eat()
    {
        // Next stage TODO: find better place for this function call
        //RulesManager.Instance.NextStage();
        // Eat the meat
        this.gameObject.SetActive(false);
    }
}
