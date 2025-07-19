using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgessGameObject;
    [SerializeField] private Image barImage;

    private IHasProgess hasProgess;
    private void Start()
    {
        hasProgess = hasProgessGameObject.GetComponent<IHasProgess>();
        hasProgess.OnProgressChanged += HasProgessOnOnProgressChanged;

        barImage.fillAmount = 0f;
        
        Hide();
    }

    private void HasProgessOnOnProgressChanged(object sender, IHasProgess.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;
        
        if (e.progressNormalized == 0f || e.progressNormalized == 1f) Hide();
        else Show();
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