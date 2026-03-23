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

/* 
Reference:
[26] J. French, How to Make an Inventory System in Unity, Game Dev Beginner, 2023. [Online]. Available: https://gamedevbeginner.com/how-to-make-an-inventory-system-in-unity/.
*/
