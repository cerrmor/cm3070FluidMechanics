using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class ButtonPadController : MonoBehaviour
{
    //ajustiable array for variable number of buttons
    public HoverButton[] hoverButton;
    
    public List<int> answersList = new List<int>();
    private List<int> buttonsPressedList = new List<int>();
    private int numCorrectAnswer = 0;
    [SerializeField] private float resetTime = 2f;
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
    }

    void Start()
    {
        //creates an event listener for each button in the array
        for (var i = 0; i < hoverButton.Length; i++)
        {
            int tempVar = i;
            //lamda function uses the index element to set the button id
            hoverButton[i].onButtonDown.AddListener((buttonId) => { OnButtonDown(tempVar); });  
        }
        Debug.Log(numCorrectAnswer);
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
                Debug.Log(buttonId);
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
            StartCoroutine(ResetButtons());
        }
        else if(!allowMultipleActivations && !hasUsedCorrectCode)
        {
            onCorrectAnswer.Invoke();
            hasUsedCorrectCode = true;

        }

    }

    private void IncorrectAnswer()
    {
        onIncorrectAnswer.Invoke();
        StartCoroutine(ResetButtons());
    }

    private void ColorSelf(Color newColor,int buttonId)
    {
        Renderer renderers = hoverButton[buttonId].GetComponentInChildren<Renderer>();
        renderers.material.color = newColor;
    }

    IEnumerator ResetButtons()
    {
        yield return new WaitForSeconds(resetTime);
        for (int i = 0; i < buttonsPressedList.Count; i++) 
        { 
            ColorSelf(Color.gray, buttonsPressedList[i]); 
        }
        buttonsPressedList.Clear();
    }
}

