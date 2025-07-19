using System;
using NUnit.Framework;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgess
{
    public event EventHandler<IHasProgess.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }
    
    [SerializeField] private FryingRecipeSO[] fryingRecipes;
    [SerializeField] private BurningRecipeSO[] burningRecipes;
    
    private State currentState;
    
    private FryingRecipeSO currentFryingRecipeSO;
    private BurningRecipeSO currentBurningRecipeSO;
    
    private float fryingTimer;
    private float burningTimer;
    
    private void Start()
    {
        currentState = State.Idle;
    }
    
    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (currentState)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgess.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / currentFryingRecipeSO.fryingTimerMax
                    });
                    if (fryingTimer > currentFryingRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(currentFryingRecipeSO.output, this);
                        currentState = State.Fried;
                        burningTimer = 0f;
                        currentBurningRecipeSO = GetBurningRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                        
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {state = currentState});
                    }  
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    
                    OnProgressChanged?.Invoke(this, new IHasProgess.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / currentBurningRecipeSO.burningTimerMax
                    });
                    
                    if (burningTimer > currentBurningRecipeSO.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(currentBurningRecipeSO.output, this);
                        currentState = State.Burned;
                        
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {state = currentState});
                        
                        OnProgressChanged?.Invoke(this, new IHasProgess.OnProgressChangedEventArgs
                        {
                            progressNormalized = 1f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (currentFryingRecipeSO == null)
                {
                    currentFryingRecipeSO = GetFryingRecipeSO(player.GetKitchenObject().GetKitchenObjectSO());
                }
                
                //Todo: Comment this if to make the game a little more fun :)
                if (currentFryingRecipeSO != null)
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    
                    currentState = State.Frying;
                    fryingTimer = 0f;
                    
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {state = currentState});
                    
                    OnProgressChanged?.Invoke(this, new IHasProgess.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / currentFryingRecipeSO.fryingTimerMax
                    });
                }
            }   
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                
                currentFryingRecipeSO = null;
                currentBurningRecipeSO = null;
                currentState = State.Idle;
                
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {state = currentState});
                
                OnProgressChanged?.Invoke(this, new IHasProgess.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        
                        currentFryingRecipeSO = null;
                        currentBurningRecipeSO = null;
                        currentState = State.Idle;
                
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {state = currentState});
                
                        OnProgressChanged?.Invoke(this, new IHasProgess.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
        }
    }
    
    private FryingRecipeSO GetFryingRecipeSO(KitchenObjectSO input)
    {
        foreach (FryingRecipeSO fryingRecipeSo in fryingRecipes )
        {
            if (fryingRecipeSo.input == input)
            {
                return fryingRecipeSo;
            }
        }
        return null; 
    }
    
    private BurningRecipeSO GetBurningRecipeSO(KitchenObjectSO input)
    {
        foreach (BurningRecipeSO burningRecipeSo in burningRecipes )
        {
            if (burningRecipeSo.input == input)
            {
                return burningRecipeSo;
            }
        }
        return null; 
    }
    
    public bool IsFried()
    {
        return currentState == State.Fried;
    }
}