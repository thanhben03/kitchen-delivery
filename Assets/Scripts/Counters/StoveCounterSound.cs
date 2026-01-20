using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStageChanged += StoveCounter_OnStageChanged;
    }

    private void StoveCounter_OnStageChanged(StoveCounter.State obj)
    {
        bool playSound = obj.Equals(StoveCounter.State.Frying) || obj.Equals(StoveCounter.State.Fried);
        if (playSound)
        {
            Debug.Log("Play Sound from Stove");
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}
