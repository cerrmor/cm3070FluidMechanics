using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;


[RequireComponent(typeof(SteamVR_LaserPointer))]
[RequireComponent(typeof(AudioSource))]
//public class VR_UI_Input : MonoBehaviour
//{
//    [SerializeField] private SteamVR_LaserPointer laserPointer;

//    private void OnEnable()
//    {
//        laserPointer = GetComponent<SteamVR_LaserPointer>();
//        //laserPointer.PointerIn -= HandlePointerIn;
//        laserPointer.PointerIn += HandlePointerIn;
//        //laserPointer.PointerOut -= HandlePointerOut;
//        laserPointer.PointerOut += HandlePointerOut;
//        //laserPointer.PointerClick -= HandleTriggerClicked;
//        laserPointer.PointerClick += HandleTriggerClicked;

//    }

//    private void HandleTriggerClicked(object sender, PointerEventArgs e)
//    {

//        if (EventSystem.current.currentSelectedGameObject != null)
//        {

//            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
//        }
//    }

//    private void HandlePointerIn(object sender, PointerEventArgs e)
//    {
//        var button = e.target.GetComponent<Button>();
//        if (button != null)
//        {
//            //hand.
//            button.Select();
//            Debug.Log("HandlePointerIn", e.target.gameObject);
//        }
//    }

//    private void HandlePointerOut(object sender, PointerEventArgs e)
//    {

//        var button = e.target.GetComponent<Button>();
//        if (button != null)
//        {
//            EventSystem.current.SetSelectedGameObject(null);
//            Debug.Log("HandlePointerOut", e.target.gameObject);
//        }
//    }
//}
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
//{
//    public SteamVR_LaserPointer laserPointer;

//    void Awake()
//    {
//        laserPointer.PointerIn += OnPointerInside;
//        laserPointer.PointerOut += OnPointerOutside;
//        laserPointer.PointerClick += OnPointerClick;
//    }

//    public void OnPointerClick(object sender, PointerEventArgs e)
//    {
//        var button = e.target.GetComponent<Button>();
//        if (EventSystem.current.currentSelectedGameObject != null)
//        {
//            button.clicked;
//            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
//        }
//        //if (e.target.name == "Cube")
//        //{
//        //    Debug.Log("Cube was clicked");
//        //}
//        //else if (e.target.name == "Button")
//        //{
//        //    Debug.Log("Button was clicked");
//        //}
//    }

//    public void OnPointerInside(object sender, PointerEventArgs e)
//    {
//        var button = e.target.GetComponent<Button>();
//        if (button != null)
//        {
//            button.Select();
//            Debug.Log("HandlePointerIn", e.target.gameObject);
//        }
//        //if (e.target.name == "Cube")
//        //{
//        //    Debug.Log("Cube was entered");
//        //}
//        //else if (e.target.name == "Button")
//        //{
//        //    Debug.Log("Button was entered");
//        //}
//    }

//    public void OnPointerOutside(object sender, PointerEventArgs e)
//    {
//        var button = e.target.GetComponent<Button>();
//        if (button != null)
//        {
//            EventSystem.current.SetSelectedGameObject(null);
//            Debug.Log("HandlePointerOut", e.target.gameObject);
//        }
//        //if (e.target.name == "Cube")
//        //{
//        //    Debug.Log("Cube was exited");
//        //}
//        //else if (e.target.name == "Button")
//        //{
//        //    Debug.Log("Button was exited");
//        //}
//    }
//}