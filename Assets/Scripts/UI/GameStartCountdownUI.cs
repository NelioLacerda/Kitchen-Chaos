using System;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private static readonly int NumberPopup = Animator.StringToHash("NumberPopup");
    [SerializeField] private TextMeshProUGUI countDownText;

    private Animator animator;
    private int previeusCountDownNumber;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        GameHandler.Instance.OnStateChange += InstanceOnOnStateChange;
        Hide();
    }

    private void InstanceOnOnStateChange(object sender, EventArgs e)
    {
        if (GameHandler.Instance.IsCountdownToStart())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        int countDownNumber = Mathf.CeilToInt(GameHandler.Instance.GetCountdownToStartTimer());
        countDownText.text = countDownNumber.ToString();

        if (previeusCountDownNumber != countDownNumber)
        {
            previeusCountDownNumber = countDownNumber;
            animator.SetTrigger(NumberPopup);  
            SoundManager.Instance.PlayCountdownSound();
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
