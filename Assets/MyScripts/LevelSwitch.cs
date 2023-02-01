using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LevelSwitch : MonoBehaviour
{
    public int currLevel = 0;

    public string[] levelNames = new string[2] { "dont know yet", "dont know yet" };

    static LevelSwitch Switch = null;

    // Start is called before the first frame update
    void Start()
    {
        if (Switch == null) Switch = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            currLevel = (currLevel + 1) % 2;
            SteamVR_LoadLevel.Begin(levelNames[currLevel]);
        }
    }
}
