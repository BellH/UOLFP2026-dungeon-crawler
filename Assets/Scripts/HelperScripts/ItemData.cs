using UnityEngine;

[CreateAssetMenu]
//Parent class for all items (data that is the same across all instances of any specific item: name, prefab, category etc.)
public class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject itemPrefab; 
    public string category; //Weapon, consumable, etc.
    
    public virtual void Use(ItemInstance itemInstance, InventoryManager inventoryManager)
    {
        //Debug.Log(itemName + " was used!");
    }
}

