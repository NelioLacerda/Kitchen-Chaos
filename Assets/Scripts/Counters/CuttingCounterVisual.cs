using System;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string CUT = "Cut";
    private static readonly int Cut = Animator.StringToHash(CUT);
    
    [SerializeField] private CuttingCounter counter;
    
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        counter.OnCut += CounterOnOnCut;
    }

    private void CounterOnOnCut(object sender, EventArgs e)
    {
        animator.SetTrigger(Cut);
    }
}