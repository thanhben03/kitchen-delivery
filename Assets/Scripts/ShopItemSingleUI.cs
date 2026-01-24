using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSingleUI : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;
    public Image iconImg;

    public void SetDataItem(ShopItemData data)
    {
        itemName.text = $"{data.itemName}";
        itemPrice.text = $"{data.price}";
        iconImg.sprite = data.icon;
    }
}
