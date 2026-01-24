using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemData", menuName = "Shop/ItemData")]
public class ShopItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int price;
}