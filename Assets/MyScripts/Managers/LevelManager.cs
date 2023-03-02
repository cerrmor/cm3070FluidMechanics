using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LevelManager : MonoBehaviour
{
    public int currLevel = 0;
    
    public string[] levelNames = new string[4] {"StartMenu" ,"Introduction","ExperimentalScene", "museum" };
    private int NoScenes = 0;
    static LevelManager instance;
 
    public static LevelManager Instance
    {
        get { return instance; }
    }
    
    void Awake()
    {
        NoScenes = levelNames.Length;
        if (instance == null) { instance = this; }
        
        else { Destroy(this.gameObject); }

        DontDestroyOnLoad(this);
    }

    public void loadLevel()
    {
        currLevel = (currLevel + 1) % NoScenes;
        SteamVR_LoadLevel.Begin(levelNames[currLevel]);
    }
}
