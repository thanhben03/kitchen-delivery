using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }
    [SerializeField] private ShopItemData[] allItems;

    private void Awake()
    {
        Instance = this;
    }


    public ShopItemData[] GetShopItemData()
    {
        return allItems;
    }
}
