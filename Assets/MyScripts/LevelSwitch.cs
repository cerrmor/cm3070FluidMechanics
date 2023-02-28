using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LevelSwitch : MonoBehaviour
{
    public int currLevel = 0;
    
    public string[] levelNames = new string[3] {"StartMenu" ,"ExperimentalScene", "museum" };
    private int NoScenes = 0;
    static LevelSwitch Switch = null;

    // Start is called before the first frame update
    void Start()
    {
        NoScenes = levelNames.Length;
        if (Switch == null) Switch = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public void loadLevel()
    {
        Debug.Log(NoScenes);
        currLevel = (currLevel + 1) % NoScenes;
        SteamVR_LoadLevel.Begin(levelNames[currLevel]);
    }
}
