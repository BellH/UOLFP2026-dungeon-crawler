using UnityEngine;

[System.Serializable]
//Parent class for all item instances (instance specific data like condition, quantity, etc.)
public class ItemInstance
{
    public ItemData itemType;

    public ItemInstance(ItemData itemData)
    {
        itemType = itemData;
    }
}
