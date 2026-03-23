using Unity.VisualScripting;
using UnityEngine;

//Health consumables
[CreateAssetMenu]
public class HealthConsumableData : ItemData 
{   
    public int healthToAdd; //Amount of health to add

    //Overrides Use() for consumable specific functionality
    public override void Use(ItemInstance itemInstance, InventoryManager inventoryManager)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>().ChangeHealth(healthToAdd);
        //remove from inventory
    }
}


