using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// SceneController: Controls changing the scenes of the game
public class SceneController : MonoBehaviour
{

    //////// Singleton shenanigans ////////
    private static SceneController _instance;
    public static SceneController Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}
        //DontDestroyOnLoad(this.gameObject);
    }

    // Hit the start button
    public void enterIntro()
    {
        SceneManager.LoadScene("IntroScreen");
    }
    // Finish the intro screen
    public void enterMainGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    // Replay the game
    public void restartGame()
    {
        SceneManager.LoadScene("StartScreen");
    }
    // win the game
    public void winGame()
    {
        SceneManager.LoadScene("WinScene");
    }
}
