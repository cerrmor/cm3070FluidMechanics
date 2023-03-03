using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;
using TMPro;

public class ButtonPadController : MonoBehaviour
{
    private Transform snapTo;

    //ajustiable array for variable number of buttons
    public HoverButton[] hoverButton;
    public GameObject prefab;
    public List<int> answersList = new List<int>();
    private List<int> buttonsPressedList = new List<int>();
    private int numCorrectAnswer = 0;
    [SerializeField] private TextMeshPro displayText;
    [SerializeField] private float resetTime = 2f;
    [SerializeField] private string questionText;
    [SerializeField] private string successText;
    [Header("Button Entry Events")]
    public UnityEvent onCorrectAnswer;
    public UnityEvent onIncorrectAnswer;

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
        GetSnapPointTransform();
        //creates an event listener for each button in the array
        for (var i = 0; i < hoverButton.Length; i++)
        {
            int tempVar = i;
            //lamda function uses the index element to set the button id
            hoverButton[i].onButtonDown.AddListener((buttonId) => { OnButtonDown(tempVar); });  
        }
    }

    private void OnButtonDown(int buttonId)
    {
        if (buttonsPressedList.Count >= numCorrectAnswer ) return;
        if (buttonsPressedList.Contains(buttonId)) return;
        else buttonsPressedList.Add(buttonId);

        //sets the color of the pressed button to cyan to indicate a selection
        for (var i = 0; i < buttonsPressedList.Count; i++)
        {
            if (buttonsPressedList[i] == buttonId)
            {
                ColorSelf(Color.cyan,buttonId);
            }
        }

        if (buttonsPressedList.Count >= numCorrectAnswer) checkAnswers();
    }

    private void checkAnswers()
    {
        for(var i = 0; i < answersList.Count; i++)
        {
            if (!answersList.Contains(buttonsPressedList[i]))
            {
                IncorrectAnswer();
                return;
            }
        }
        CorrectAnswer();
    }

    private void CorrectAnswer()
    {
        if(allowMultipleActivations)
        {
            onCorrectAnswer.Invoke();
            displayText.color = Color.yellow;
            displayText.text = successText;
            StartCoroutine(ResetButtons());
        }
        else if(!allowMultipleActivations && !hasUsedCorrectCode)
        {
            onCorrectAnswer.Invoke();
            hasUsedCorrectCode = true;
            displayText.color = Color.yellow;
            displayText.text = successText;
            StartCoroutine(DoTeleportOrb());
        }

    }

    private void IncorrectAnswer()
    {
        onIncorrectAnswer.Invoke();
        displayText.color = Color.red;
        StartCoroutine(ResetButtons());
    }

    private void ColorSelf(Color newColor,int buttonId)
    {
        Renderer renderers = hoverButton[buttonId].GetComponentInChildren<Renderer>();
        renderers.material.color = newColor;
    }

    void GetSnapPointTransform()
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

    IEnumerator ResetButtons()
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

    private IEnumerator DoTeleportOrb()
    {
        GameObject Orb = GameObject.Instantiate<GameObject>(prefab);
        Orb.transform.position = snapTo.position;
        Orb.transform.rotation = Quaternion.Euler(0, Random.value * 360f, 0);

        

        Rigidbody rigidbody = Orb.GetComponent<Rigidbody>();
        if (rigidbody != null)
            rigidbody.isKinematic = true;


        Vector3 initialScale = Vector3.one * 0.01f;
        Vector3 targetScale = Vector3.one * (0.3f + (Random.value * 0.25f));

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

