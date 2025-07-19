using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    private static readonly int IsWalking = Animator.StringToHash(IS_WALKING);
    
    private Animator animator;
    
    [SerializeField] private Player player;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IsWalking, player.IsWalking());
    }
}