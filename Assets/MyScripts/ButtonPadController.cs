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
    [SerializeField] private float resetTime = 2f;
    [SerializeField] private string successText;
    [Header("Button Entry Events")]
    public UnityEvent onCorrectAnswer;
    public UnityEvent onIncorrectAnswer;

    public bool allowMultipleActivations = false;
    private bool hasUsedCorrectCode = false;
    public bool HasUsedCorrectCode { get { return hasUsedCorrectCode; } }

    //prefab for testing purpose only
    public GameObject prefab;

    void Start()
    {
        //creates an event listener for each button in the array
        for (var i = 0; i < hoverButton.Length; i++)
        {
            var temp = i;
            hoverButton[i].onButtonDown.AddListener((buttonId) => { OnButtonDown(temp); }); 
            //hoverButton[i].onButtonDown.AddListener(OnButtonDown); 
        }
        
    }

    private void doSomething(int button)
    {
        Debug.Log(button);
    }
    private void OnButtonDown(int buttonId)
    {
        //if (buttonsPressedList.Count >= 2) return;
        //buttonsPressedList.Add(selectedButton);

        //Debug.Log(get);
        Debug.Log(buttonId);
        StartCoroutine(DoPlant());
    }

    //will generate a flower of random colour for every button press
    private IEnumerator DoPlant()
    {
        GameObject planting = GameObject.Instantiate<GameObject>(prefab);
        planting.transform.position = this.transform.position;
        planting.transform.rotation = Quaternion.Euler(0, Random.value * 360f, 0);

        planting.GetComponentInChildren<MeshRenderer>().material.SetColor("_TintColor", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));

        Rigidbody rigidbody = planting.GetComponent<Rigidbody>();
        if (rigidbody != null)
            rigidbody.isKinematic = true;


        Vector3 initialScale = Vector3.one * 0.01f;
        Vector3 targetScale = Vector3.one * (1 + (Random.value * 0.25f));

        float startTime = Time.time;
        float overTime = 0.5f;
        float endTime = startTime + overTime;

        while (Time.time < endTime)
        {
            planting.transform.localScale = Vector3.Slerp(initialScale, targetScale, (Time.time - startTime) / overTime);
            yield return null;
        }


        if (rigidbody != null)
            rigidbody.isKinematic = false;
    }
}

