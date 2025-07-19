using System;
using UnityEngine;

public class StoveBurningWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounterOnOnProgressChanged;
        Hide();
    }

    private void StoveCounterOnOnProgressChanged(object sender, IHasProgess.OnProgressChangedEventArgs e)
    {
        float burnShowProgressNormalized = .5f;
        bool show = e.progressNormalized >= burnShowProgressNormalized && stoveCounter.IsFried();
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);   
    }
}