using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconContainer.gameObject.SetActive(false);
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSO)
    {
        iconContainer.gameObject.SetActive(true);

        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        
        iconTemplate.gameObject.SetActive(true);
        recipeNameText.text = recipeSO.recipeName;

        foreach (var kitchenObject in recipeSO.ingredients)
        {
            var iconTransfrom = Instantiate(iconTemplate, iconContainer);
            iconTransfrom.GetComponent<Image>().sprite = kitchenObject.sprite;
        }
        iconTemplate.gameObject.SetActive(false);
    }
}