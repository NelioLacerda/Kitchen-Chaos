using System;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAltText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;

    private void Start()
    {
        GameInput.Instance.OnBidingRebind += InstanceOnOnBidingRebind;
        GameHandler.Instance.OnStateChange += InstanceOnOnStateChange;
        
        UpdateVisual();
        
        Show();
    }

    private void InstanceOnOnStateChange(object sender, EventArgs e)
    {
        if (GameHandler.Instance.IsCountdownToStart())
        {
            Hide();
        }
    }

    private void InstanceOnOnBidingRebind(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        moveUpText.text = GameInput.Instance.GetBidingText(GameInput.Biding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBidingText(GameInput.Biding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBidingText(GameInput.Biding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBidingText(GameInput.Biding.Move_Right);
        interactText.text = GameInput.Instance.GetBidingText(GameInput.Biding.Interact);
        interactAltText.text = GameInput.Instance.GetBidingText(GameInput.Biding.Interact_Alternate);
        pauseText.text = GameInput.Instance.GetBidingText(GameInput.Biding.Pause);
        gamepadInteractText.text = GameInput.Instance.GetBidingText(GameInput.Biding.Gamepad_Interact);
        gamepadInteractAltText.text = GameInput.Instance.GetBidingText(GameInput.Biding.Gamepad_Interact_Alternate);
        gamepadPauseText.text = GameInput.Instance.GetBidingText(GameInput.Biding.Gamepad_Pause);
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