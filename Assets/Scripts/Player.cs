using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;    
    [SerializeField] private Transform kitchenObjectHoldPoint;
    
    private bool isWalking;
    private Vector3 lastMoveDirection;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null) Debug.Log("Error");
        Instance = this;
    }
    
    private void Start()
    {
        gameInput.OnInteractAction += GameInputOnOnInteractAction;
        gameInput.OnInteractAlternativeAction += GameInputOnAlternateIterationAction;
    }

    private void GameInputOnAlternateIterationAction(object sender, EventArgs e)
    {
        if (!GameHandler.Instance.IsInGame()) return;
        
        if (selectedCounter != null)
            selectedCounter.InteractAlternate(this);
    }

    private void GameInputOnOnInteractAction(object sender, EventArgs e)
    {
        if (!GameHandler.Instance.IsInGame()) return;

        if (selectedCounter != null)
            selectedCounter.Interact(this);
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        var input = gameInput.GetMovementVectorNormalized();
        
        var moveDir = new  Vector3(input.x, 0f, input.y);
        
        if (moveDir != Vector3.zero) lastMoveDirection = moveDir;
        
        var interactionDistance = 2f;
        if (Physics.Raycast(transform.position, lastMoveDirection, 
                out RaycastHit hit, interactionDistance, countersLayerMask))
        {
            if (hit.transform.TryGetComponent(out BaseCounter clearCounter))
            {
                //has clearCounter
                if (clearCounter != selectedCounter) SetSelectedCounter(clearCounter);
            }
            else SetSelectedCounter(null);
        } else SetSelectedCounter(null);
    }
    
    private void HandleMovement()
    {
        var input = gameInput.GetMovementVectorNormalized();
        
        var moveDir = new  Vector3(input.x, 0f, input.y);

        var moveDistance = moveSpeed * Time.deltaTime;
        var playerHeight = 2f;
        var playerRadius = 0.7f;
        var canMove = !Physics.CapsuleCast(transform.position, 
            transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //Cannot move towards moveDir
            
            //Try to move only on x direction.
            var moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = (moveDir.x < -0.5f || moveDir.x > 0.5f) && !Physics.CapsuleCast(transform.position, 
                transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                //Cannot move on x
                
                //Try to move on z
                var moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = (moveDir.z < -0.5f || moveDir.z > 0.5f) && !Physics.CapsuleCast(transform.position, 
                    transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //Can move on z
                    moveDir = moveDirZ;
                }
            }
        }
        if (canMove) transform.position += moveDir * moveDistance;
        
        isWalking = moveDir != Vector3.zero;
        
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter clearCounter)
    {
        selectedCounter = clearCounter;
        OnSelectedCounterChanged?.Invoke(this, 
            new OnSelectedCounterChangedEventArgs {selectedCounter = selectedCounter});
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
} 