using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //////// Singleton shenanigans ////////
    private static AudioManager _instance;
    public static AudioManager Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    // BGM


    // SFX
    public AudioSource eat;
    public AudioSource jump;
    public AudioSource switchSound;

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}

        DontDestroyOnLoad(this.gameObject);
    }

    public void Jump()
    {
        jump.Play();
    }
    public void Eat()
    {
        eat.Play();
    }
    public void Switch()
    {
        switchSound.Play();
    }

}
