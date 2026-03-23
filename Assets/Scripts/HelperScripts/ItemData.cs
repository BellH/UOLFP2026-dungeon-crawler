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

/* 
Reference:
[26] J. French, How to Make an Inventory System in Unity, Game Dev Beginner, 2023. [Online]. Available: https://gamedevbeginner.com/how-to-make-an-inventory-system-in-unity/.
*/

