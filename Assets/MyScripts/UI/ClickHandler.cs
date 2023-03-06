using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private int CreditsSceneArrayIndex = 0;
    [SerializeField] private Button StartButton, CreditsButton, ExitButton;
    // Start is called before the first frame update
    private void Awake()
    {
        StartButton.onClick.AddListener(StartTask);
        CreditsButton.onClick.AddListener(CreditsTask);
        ExitButton.onClick.AddListener(EndTask);
    }

    private void StartTask ()
    {
        LevelManager.Instance.loadLevel();
    }
    private void CreditsTask ()
    {
        LevelManager.Instance.loadSpecificLevel(CreditsSceneArrayIndex);
    }
    private void EndTask ()
    {
        GameManager.Instance.ExitGame();
    }
}
