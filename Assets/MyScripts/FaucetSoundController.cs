using System.Collections;
using System.Collections.Generic;
using Valve.VR.InteractionSystem;
using UnityEngine;

public class FaucetSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip faucetOn;
    [SerializeField] private AudioClip faucetRunning;
    [SerializeField] private AudioClip faucetOff;
    [SerializeField] private GameObject faucet;
    [SerializeField] private CircularDrive faucetKnob;
    [SerializeField] private string faucetName;
    private AudioSource audioSource;
    private float startAngle = 0.0f;
    private bool playedOn = false;
    private bool playedRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.Find(faucetName).GetComponent<AudioSource>();
        faucetKnob = GetComponent<CircularDrive>();
        //startAngle = faucetKnob.outAngle;
        //Debug.Log(startAngle);
    }

    
    // Update is called once per frame
    public void StartFaucet()
    {
        audioSource.PlayOneShot(faucetOn, 0.7f);
        playedOn = true;
        startAngle = 0.2f;
    }

    private void RunFaucet()
    {
        playedRunning = true;
        audioSource.clip = faucetRunning;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void FaucetOff()
    {
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.PlayOneShot(faucetOff, 0.7f);
        startAngle = 0.0f;
        playedOn=false;
        playedRunning=false;
    }

    private void Update()
    {
        if(faucetKnob != null)
        {
            if(faucetKnob.outAngle > startAngle && !playedOn)
            {
                StartFaucet();
            } 
            else if(faucetKnob.outAngle > 0.0 && playedOn && !playedRunning)
            {
                RunFaucet();
            } 
            else if(faucetKnob.outAngle < startAngle && playedOn && playedRunning)
            {
                FaucetOff();
            }
        }
    }
}
