using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class FaucetSoundControllerEmitterOnly : MonoBehaviour
{
    [SerializeField] private ObiEmitter emi;

    [SerializeField] private AudioClip faucetOn;
    [SerializeField] private AudioClip faucetRunning;
    [SerializeField] private AudioClip faucetOff;

    private AudioSource audioSource;
    private bool playedOn = false;
    private bool playedRunning = false;



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        emi = GetComponent<ObiEmitter>();        
    }


    // Update is called once per frame
    public void StartFaucet()
    {
        audioSource.PlayOneShot(faucetOn,1.0f);
        playedOn = true;
    }

    private void RunFaucet()
    {
        playedRunning = true;
        audioSource.clip = faucetRunning;
        audioSource.volume = 1.0f;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void FaucetOff()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.PlayOneShot(faucetOff,1.0f);
        playedOn = false;
        playedRunning = false;
    }

    private void Update()
    {
        if (emi != null)
        {
            if (emi.isEmitting && !playedOn)
            {
                StartFaucet();
            }
            else if (emi.isEmitting && playedOn && !playedRunning)
            {
                RunFaucet();
            }
            else if (!emi.isEmitting && playedOn && playedRunning)
            {
                FaucetOff();
            }
        }
    }
}
