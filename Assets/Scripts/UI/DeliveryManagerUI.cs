using System;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += InstanceOnOnRecipeSpawned;
        DeliveryManager.Instance.OnRecipeDelivered += InstanceOnOnRecipeDelivered;
        
        UpdateVisual();
    }

    private void InstanceOnOnRecipeDelivered(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void InstanceOnOnRecipeSpawned(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }
        
        recipeTemplate.gameObject.SetActive(true);

        foreach (var recipeSO in DeliveryManager.Instance.GetWaitingRecipes())
        {
            Transform recipeTramsform = Instantiate(recipeTemplate, container);
            recipeTemplate.gameObject.SetActive(true);
            recipeTramsform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
        recipeTemplate.gameObject.SetActive(false);
    }
}