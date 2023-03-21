using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;
using TMPro;

public class TransportationSphereSelector : MonoBehaviour
{
    private Transform snapTo;

    //ajustiable array for variable number of buttons
    public HoverButton[] hoverButton;
    public GameObject prefab1;
    public GameObject prefab2;
    private List<GameObject> orbList = new List<GameObject>();
    private bool spawnedOrb1 = false;
    private bool spawnedOrb2 = false;
    public bool SpawnTeleportOrb = true;
    public List<int> answersList = new List<int>();
    private List<int> buttonsPressedList = new List<int>();
    private int numCorrectAnswer = 0;
    [SerializeField] private TextMeshPro displayText;
    [SerializeField] private float resetTime = 2f;
    [SerializeField] private string questionText;
    [SerializeField] private string successText;
    [Header("Button Entry Events")]
    public UnityEvent onButtonPressed;
    public UnityEvent onOtherButtonPressed;

    public bool allowMultipleActivations = false;
    private bool hasUsedCorrectCode = false;
    public bool HasUsedCorrectCode { get { return hasUsedCorrectCode; } }

    private void Awake()
    {
        numCorrectAnswer = answersList.Count;
        displayText.text = questionText;
    }

    void Start()
    {
        GetSnapToPointTransform();
        //creates an event listener for each button in the array
        for (var i = 0; i < hoverButton.Length; i++)
        {
            int tempVar = i;
            //lamda function uses the index element to set the button id
            hoverButton[i].onButtonDown.AddListener((buttonId) => { OnButtonPressed(tempVar); });  
        }
    }

    private void OnButtonPressed(int buttonId)
    {
        GameObject temp = null;
        string orbName = "";
        int pressedButton = 0;
        if (buttonsPressedList.Count >= numCorrectAnswer ) return;
        if (buttonsPressedList.Contains(buttonId)) return;
        else buttonsPressedList.Add(buttonId);

        //sets the color of the pressed button to cyan to indicate a selection
        for (var i = 0; i < buttonsPressedList.Count; i++)
        {
            if (buttonsPressedList[i] == buttonId)
            {
                ColorSelf(Color.cyan,buttonId);
                if (buttonId == 0)
                {
                    orbName = "buoyancy";
                    temp = prefab1;
                    pressedButton = buttonId;
                }
                else
                {
                    orbName = "viscosity";
                    temp = prefab2;
                    pressedButton = buttonId;
                }
            }
        }

        checkButtonSelected(temp,orbName);
        //if (buttonsPressedList.Count >= numCorrectAnswer) checkButtonSelected(orb);
    }

    private void checkButtonSelected(GameObject prefab,string orbName)
    {
        if(orbList.Count >= 1)SwitchSelection(prefab.tag,orbName);
        TransportSphereSelected(prefab,orbName);
    }

    private void TransportSphereSelected (GameObject prefab, string orbName)
    {
        if(allowMultipleActivations)
        {
            onButtonPressed.Invoke();
            displayText.color = Color.yellow;
            displayText.text = successText;
            
            if (prefab == prefab1 && !spawnedOrb1) StartCoroutine(TeleportOrb(prefab));
            else if (prefab == prefab2 && !spawnedOrb2) StartCoroutine(TeleportOrb(prefab));
            //if (orbList.Count >= 2 && (spawnedOrb1 || spawnedOrb2)) SwitchSelection(prefab.tag, orbName);

            StartCoroutine(ResetSelectedButton());
        }
        else if(!allowMultipleActivations && !hasUsedCorrectCode)
        {
            onButtonPressed.Invoke();
            hasUsedCorrectCode = true;
            displayText.color = Color.yellow;
            displayText.text = successText;
            if (SpawnTeleportOrb)StartCoroutine(TeleportOrb(prefab));
        }

    }

    private void SwitchSelection(string orb1,string orb2)
    {
        for(var i = 0; i < orbList.Count; i++)
        {
            if (orbList[i].CompareTag(orb2))
            {
                if (orbList[i].activeSelf) orbList[i].SetActive(!orbList[i].activeSelf);
            }

            else if (orbList[i].CompareTag(orb1))
            {
               if (!orbList[i].activeSelf) orbList[i].SetActive(!orbList[i].activeSelf);
            }
        }
    }

    private void ColorSelf(Color newColor,int buttonId)
    {
        Renderer renderers = hoverButton[buttonId].GetComponentInChildren<Renderer>();
        renderers.material.color = newColor;
    }

    void GetSnapToPointTransform()
    {
        GameObject go = GameObject.Find("SnapPoint");
        if (go != null)
        {
            snapTo = go.transform;
        }
        else
        {
            Debug.Log("SnapPoint not found");
        }
    }

    IEnumerator ResetSelectedButton()
    {
        yield return new WaitForSeconds(resetTime);
        for (int i = 0; i < buttonsPressedList.Count; i++) 
        { 
            ColorSelf(Color.gray, buttonsPressedList[i]); 
        }
        buttonsPressedList.Clear();
        displayText.color = Color.white;
        displayText.text = questionText;
    }

    private IEnumerator TeleportOrb(GameObject prefab)
    {
        if (prefab == prefab1) spawnedOrb1 = true;
        else if (prefab == prefab2) spawnedOrb2 = true;
        GameObject Orb = GameObject.Instantiate<GameObject>(prefab);
       
        Orb.transform.position = snapTo.position;
        Orb.transform.rotation = Quaternion.Euler(0, Random.value * 360f, 0);

        orbList.Add(Orb);

        Rigidbody rigidbody = Orb.GetComponent<Rigidbody>();
        if (rigidbody != null)
            rigidbody.isKinematic = true;


        Vector3 initialScale = Vector3.one * 0.01f;
        Vector3 targetScale = Vector3.one * (0.32f );

        float startTime = Time.time;
        float overTime = 0.5f;
        float endTime = startTime + overTime;

        while (Time.time < endTime)
        {
            Orb.transform.localScale = Vector3.Slerp(initialScale, targetScale, (Time.time - startTime) / overTime);
            yield return null;
        }


        if (rigidbody != null)
            rigidbody.isKinematic = false;
       
    }
}
