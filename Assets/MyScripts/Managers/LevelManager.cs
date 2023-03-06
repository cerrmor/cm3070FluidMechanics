using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LevelManager : MonoBehaviour
{
    public int currLevel = 0;
    public string[] levelNames = new string[6] {"StartMenu" ,"Introduction", "ScienceLabHub", "BuoyancyAndDisplacementLab", "ViscosityLab", "Credits" };
    private int NoScenes = 0;
    static LevelManager instance;
 
    public static LevelManager Instance
    {
        get { return instance; }
    }
    
    void Awake()
    {
        NoScenes = levelNames.Length;
        if (instance == null) { instance = this; Debug.Log(instance); }
        
        else { Destroy(this.gameObject); }

        DontDestroyOnLoad(this);
    }

    public void loadLevel()
    {
        currLevel = (currLevel + 1) % NoScenes;
        SteamVR_LoadLevel.Begin(levelNames[currLevel]);
    }
    public void loadSpecificLevel(int level)
    {
        currLevel = (level) % NoScenes;
        Debug.Log("LOADING LEVEL");
        Debug.Log(currLevel);
        Debug.Log(levelNames[currLevel]);
        SteamVR_LoadLevel.Begin(levelNames[currLevel]);
    }

    //private void OnTriggerEnter(Collider ChangeScene)
    //{
    //    if (ChangeScene.gameObject.CompareTag("MainCamera"))
    //    {
    //        loadLevel();
    //    }
    //}
}
