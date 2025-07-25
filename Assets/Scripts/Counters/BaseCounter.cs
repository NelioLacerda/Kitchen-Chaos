using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlaceHere;
    
    public static void ResetStaticData()
    {
        OnAnyObjectPlaceHere = null;
    }
    
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;
    
    public virtual void Interact(Player player)
    {
        
    }
    
    public virtual void InteractAlternate(Player player)
    {
        
    }
    
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject)
        {
            OnAnyObjectPlaceHere?.Invoke(this, EventArgs.Empty);
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