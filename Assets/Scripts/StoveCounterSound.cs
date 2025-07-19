using System;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    
    private AudioSource audioSource;
    
    private float warningSoundTimer;
    private bool playWarningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounterOnOnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounterOnOnProgressChanged;
    }
    
    private void Update()
    {
        if (playWarningSound)
        {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0f)
            {
                var warningSoundTimerMax = .2f;
                warningSoundTimer = warningSoundTimerMax;
                
                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }

    private void StoveCounterOnOnProgressChanged(object sender, IHasProgess.OnProgressChangedEventArgs e)
    {
        float burnShowProgressNormalized = .5f;
        playWarningSound = e.progressNormalized >= burnShowProgressNormalized && stoveCounter.IsFried();
    }

    private void StoveCounterOnOnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Fried || e.state == StoveCounter.State.Frying
                                                             || e.state == StoveCounter.State.Burned;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();   
        }
    }
}