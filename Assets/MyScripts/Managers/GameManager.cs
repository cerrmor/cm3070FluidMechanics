using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton manager
    #region Singleton
    static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if(instance == null) { instance = this; }
        
        else { Destroy(this.gameObject); }
        
        DontDestroyOnLoad(this);
    }
    #endregion
}
