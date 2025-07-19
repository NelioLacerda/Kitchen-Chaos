using System;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter counter;
    [SerializeField] private GameObject[] visualGameObject;

    
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += PlayerOnOnSelectedCounterChanged;
    }

    private void PlayerOnOnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (counter.Equals(e.selectedCounter)) Show();
        else Hide();
    }

    private void Show()
    {
        foreach (GameObject gameObject in  visualGameObject)
        {
            gameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject gameObject in  visualGameObject)
        {
            gameObject.SetActive(false);
        }
    }
}