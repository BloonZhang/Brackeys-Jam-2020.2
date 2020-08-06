using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatController : MonoBehaviour
{

    // static list that holds all meats
    public static List<MeatController> meatList;

    // private variables
    private Vector3 spawnLocation;

    void Awake()
    {
        if (meatList == null) {meatList = new List<MeatController>();}
        meatList.Add(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = this.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        meatList.Remove(this);
    }

    // public methods
    public void Reset()
    {
        // Spawn a copy and delete current one
        MeatController newMeat = Instantiate(this, spawnLocation, Quaternion.identity);
        newMeat.gameObject.name = this.gameObject.name;
        Destroy(this.gameObject);
    }
    public void Eat()
    {
        // Next stage TODO: find better place for this function call
        RulesManager.Instance.NextStage();
        // Eat the meat
        Destroy(this.gameObject);
    }
}
