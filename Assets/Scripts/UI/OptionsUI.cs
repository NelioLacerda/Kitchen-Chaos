using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }
    
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadInteractAltButton;
    [SerializeField] private Button gamepadPauseButton;
    
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
    
    [SerializeField] private Transform pressToRebindKeyTransform;

    private Action OnCloseButtonAction;
    
    private void Awake()
    {
        Instance = this;
        
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();       
        });
        
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            OnCloseButtonAction();
        });
        
        moveUpButton.onClick.AddListener(() => { RebindBiding(GameInput.Biding.Move_Up); });
        moveDownButton.onClick.AddListener(() => { RebindBiding(GameInput.Biding.Move_Down); });
        moveLeftButton.onClick.AddListener(() => { RebindBiding(GameInput.Biding.Move_Left); });
        moveRightButton.onClick.AddListener(() => { RebindBiding(GameInput.Biding.Move_Right); });
        interactButton.onClick.AddListener(() => { RebindBiding(GameInput.Biding.Interact); });
        interactAltButton.onClick.AddListener(() => { RebindBiding(GameInput.Biding.Interact_Alternate); });
        pauseButton.onClick.AddListener(() => { RebindBiding(GameInput.Biding.Pause); });
        gamepadInteractButton.onClick.AddListener(() => { RebindBiding(GameInput.Biding.Gamepad_Interact); });
        gamepadInteractAltButton.onClick.AddListener(() => { RebindBiding(GameInput.Biding.Gamepad_Interact_Alternate); });
        gamepadPauseButton.onClick.AddListener(() => { RebindBiding(GameInput.Biding.Gamepad_Pause); });
    }

    private void Start()
    {
        GameHandler.Instance.OnGameUnPaused += InstanceOnOnGameUnPaused;
        UpdateVisual();

        HidePressToRebindKey();
        Hide();
    }

    private void InstanceOnOnGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

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

    public void Show(Action OnCloseButtonAction)
    {
        this.OnCloseButtonAction = OnCloseButtonAction;
        
        gameObject.SetActive(true);
        
        soundEffectsButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);   
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);   
    }
    
    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);   
    }

    private void RebindBiding(GameInput.Biding biding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBiding(biding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();       
        });
    }
}