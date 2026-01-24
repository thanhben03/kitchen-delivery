using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform shopItemTemplate;

    private bool isOpen = true;


    void Start()
    {
        GameInput.Instance.OnOpenShopAction += GameInput_OnOpenShopAction;
        gameObject.SetActive(false);
    }

    private void GameInput_OnOpenShopAction(object sender, System.EventArgs e)
    {
        isOpen = !isOpen;
        ToggleShowShop();
        UpdateVisual();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleShowShop()
    {
        gameObject.SetActive(!isOpen);
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == shopItemTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        foreach (ShopItemData shopItemData in ShopManager.Instance.GetShopItemData())
        {
            Transform shopItemTransform = Instantiate(shopItemTemplate, container);
            shopItemTransform.gameObject.SetActive(true);
            shopItemTransform.GetComponent<ShopItemSingleUI>().SetDataItem(shopItemData);

        }
    }
}
