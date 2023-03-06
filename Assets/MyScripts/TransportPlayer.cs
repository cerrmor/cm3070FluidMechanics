using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TransportPlayer : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 3f;
    [SerializeField] private float Wait_Time = 3f;
    [SerializeField] private bool SelectLevel = false;
    [SerializeField] private int Level = 0;
    
    private void OnTriggerEnter(Collider ChangeScene)
    {
        
        if (ChangeScene.gameObject.CompareTag("MainCamera"))
        {
            StartCoroutine(StartCounting());
            FadeToBlack();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
        FadeFromW();
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void FadeToBlack()
    {
        //set start color
        //SteamVR_Fade.Start(Color.clear, 0f);
        SteamVR_Fade.View(Color.black, fadeDuration);
        //set and start fade to
        //SteamVR_Fade.Start(Color.black, fadeDuration);
    }
    private void FadeFromW()
    {
        //set start color
        //SteamVR_Fade.Start(Color.clear, 0f);
        SteamVR_Fade.View(Color.clear, 0f);
        //set and start fade to
        //SteamVR_Fade.Start(Color.clear, fadeDuration);
    }

    IEnumerator StartCounting()
    {
        float waitTime = Wait_Time;
        yield return wait(waitTime);  
    }

    IEnumerator wait(float waitTime)
    {
        float counter = 0;
        while (counter < waitTime)
        {
            //Increment Timer until counter >= waitTime
            counter += Time.deltaTime;
            Debug.Log("We have waited for: " + counter + " seconds");
            if(counter >= waitTime)
            {
                yield return ChangeLevel();
            }
            yield return null;
        }

    }

    IEnumerator ChangeLevel()
    {
        Debug.Log("the player has changed level");
        if(!SelectLevel) LevelManager.Instance.loadLevel();
        else LevelManager.Instance.loadSpecificLevel(Level);
        yield return null;
    }




}
