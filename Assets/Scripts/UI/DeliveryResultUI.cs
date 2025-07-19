using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private static readonly int Popup = Animator.StringToHash("Popup");
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    
    [SerializeField] private Color successColor;
    [SerializeField] private Color failColor;
    
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failSprite;
    
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeDeliveredSuccess += InstanceOnOnRecipeDeliveredSuccess;
        DeliveryManager.Instance.OnRecipeDeliveredFailed += InstanceOnOnRecipeDeliveredFailed;
        
        gameObject.SetActive(false);
    }

    private void InstanceOnOnRecipeDeliveredFailed(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        
        animator.SetTrigger(Popup);
        backgroundImage.color = failColor;
        iconImage.sprite = failSprite;
        messageText.text = "DELIVERY\nFAILED";
    }

    private void InstanceOnOnRecipeDeliveredSuccess(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        
        animator.SetTrigger(Popup);
        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        messageText.text = "DELIVERY\nSUCCESS";
    }
}