using System;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";
    private static readonly int OpenClose = Animator.StringToHash(OPEN_CLOSE);
    
    [SerializeField] private ContainerCounter counter;
    
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        counter.OnPlayerGrabbedObject += CounterOnOnPlayerGrabbedObject;
    }

    private void CounterOnOnPlayerGrabbedObject(object sender, EventArgs e)
    {
        animator.SetTrigger(OpenClose);
    }
}