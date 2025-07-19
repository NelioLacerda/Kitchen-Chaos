using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgess
{
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgess.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;
    
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;
    
    private int cuttingProgress;
    private CuttingRecipeSO currentCuttingRecipeSO;
    
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                currentCuttingRecipeSO = GetCuttingRecipeSO(player.GetKitchenObject().GetKitchenObjectSO());
                
                //Todo: Comment this if to make the game a little more fun :)
                if (currentCuttingRecipeSO != null)
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    OnProgressChanged?.Invoke(this, new IHasProgess.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float) cuttingProgress / currentCuttingRecipeSO.cuttingProgressMax
                    });
                }
            }   
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                cuttingProgress = 0;
                
                OnProgressChanged?.Invoke(this, new IHasProgess.OnProgressChangedEventArgs
                {
                    progressNormalized = (float) cuttingProgress / currentCuttingRecipeSO.cuttingProgressMax
                });
                currentCuttingRecipeSO = null;
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && currentCuttingRecipeSO != null)
        {
            cuttingProgress++;
            
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);
            
            OnProgressChanged?.Invoke(this, new IHasProgess.OnProgressChangedEventArgs
            {
                progressNormalized = (float) cuttingProgress / currentCuttingRecipeSO.cuttingProgressMax
            });
            
            if (cuttingProgress >= currentCuttingRecipeSO.cuttingProgressMax)
            {
                GetKitchenObject().DestroySelf();
            
                KitchenObject.SpawnKitchenObject(currentCuttingRecipeSO.output, this);
            }
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSO(KitchenObjectSO input)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOs )
        {
            if (cuttingRecipeSO.input == input)
            {
                return cuttingRecipeSO;
            }
        }
        return null; 
    }
}