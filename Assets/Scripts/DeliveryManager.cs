using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeDelivered;
    public event EventHandler OnRecipeDeliveredFailed;
    public event EventHandler OnRecipeDeliveredSuccess;
    
    public static DeliveryManager Instance { get; private set; }
    
    [SerializeField] private RecipeListSO recipeList;
    
    private List<RecipeSO> watingRecipes;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int deliveryRecipes = 0;
    
    private void Awake()
    {
        Instance = this;
        watingRecipes = new List<RecipeSO>(waitingRecipesMax);
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (watingRecipes.Count < waitingRecipesMax && GameHandler.Instance.IsInGame())
            {
                var waitingRecipeSO = recipeList.recipes[Random.Range(0, recipeList.recipes.Count)];
                watingRecipes.Add(waitingRecipeSO);
                
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < watingRecipes.Count; i++)
        {
            var recipeSO = watingRecipes[i];
            if (recipeSO.ingredients.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateCountentsMatchesRecipe = true;
                //Has the same number of ingredients
                foreach (var recipeIngredient in recipeSO.ingredients)
                {
                    bool ingredientFound = false;
                    foreach (var plateKitchanObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchanObjectSO == recipeIngredient)
                        {
                           ingredientFound = true;
                           break;
                        }
                    }
                    if (!ingredientFound)
                    {
                       plateCountentsMatchesRecipe = false; 
                    }
                }

                if (plateCountentsMatchesRecipe)
                {
                    watingRecipes.RemoveAt(i);
                    deliveryRecipes++;
                    
                    OnRecipeDelivered?.Invoke(this, EventArgs.Empty);
                    OnRecipeDeliveredSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        //No matches found
        OnRecipeDeliveredFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipes()
    {
        return watingRecipes;   
    }
    
    public int GetDeliveryRecipes()
    {
        return deliveryRecipes;   
    }
}