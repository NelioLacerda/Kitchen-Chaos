using System;
using UnityEngine;

public class StoveBurnFalshUI : MonoBehaviour
{
    private static readonly int IsFlashing = Animator.StringToHash("IsFlashing");
    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounterOnOnProgressChanged;
        animator.SetBool(IsFlashing, false);
    }

    private void StoveCounterOnOnProgressChanged(object sender, IHasProgess.OnProgressChangedEventArgs e)
    {
        float burnShowProgressNormalized = .5f;
        bool show = e.progressNormalized >= burnShowProgressNormalized && stoveCounter.IsFried();
        
        animator.SetBool(IsFlashing, show);
    }
    
}