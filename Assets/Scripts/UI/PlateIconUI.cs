using System;
using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;
    
    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    
    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnOnIngredientAdded;
        UpdateVisual();
    }

    private void PlateKitchenObjectOnOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        
        iconTemplate.gameObject.SetActive(true);
        
        foreach (KitchenObjectSO kitchenObject in plateKitchenObject.GetKitchenObjectSOList())
        {
            Transform iconTransfrom = Instantiate(iconTemplate, transform);
            //iconTemplate.gameObject.SetActive(true);
            iconTransfrom.GetComponent<PlateIconSingleUI>().SetKitchenObject(kitchenObject);
        }
        iconTemplate.gameObject.SetActive(false);
    }
}