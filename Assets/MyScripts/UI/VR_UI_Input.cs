using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;


[RequireComponent(typeof(SteamVR_LaserPointer))]
[RequireComponent(typeof(AudioSource))]

public class VR_UI_Input : MonoBehaviour
{
    private SteamVR_LaserPointer laserPointer;

    //	Sound Fx
    [SerializeField] private AudioClip enterButton;
    [SerializeField] private AudioClip exitButton;

    private AudioSource audioSource;
    private void OnEnable()
    {
        laserPointer = GetComponent<SteamVR_LaserPointer>();
        audioSource = GetComponent<AudioSource>();
        
        laserPointer.PointerIn += OnPointerIn;
        laserPointer.PointerOut += OnPointerOut;
        laserPointer.PointerClick += OnPointerClicked;

    }

    private void OnPointerClicked(object sender, PointerEventArgs e)
    {
        IPointerClickHandler clickHandler = e.target.GetComponent<IPointerClickHandler>();   
        if (clickHandler == null)
        {
            Debug.Log("ClickHandler=null");
            return;
        }
        clickHandler.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    private void OnPointerIn(object sender, PointerEventArgs e)
    {
        IPointerEnterHandler pointerEnterHandler = e.target.GetComponent<IPointerEnterHandler>();
        if (pointerEnterHandler == null)
        {
            return;
        }
        audioSource.PlayOneShot(enterButton, 0.5f);
        pointerEnterHandler.OnPointerEnter(new PointerEventData(EventSystem.current));
    }

    private void OnPointerOut(object sender, PointerEventArgs e)
    {
        IPointerExitHandler pointerExitHandler = e.target.GetComponent<IPointerExitHandler>();
        if (pointerExitHandler == null)
        {
            return;
        }
        audioSource.PlayOneShot(exitButton,1f);
        pointerExitHandler.OnPointerExit(new PointerEventData(EventSystem.current));
    }
}
