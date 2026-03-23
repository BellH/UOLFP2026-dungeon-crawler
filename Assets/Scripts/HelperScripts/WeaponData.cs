using Unity.VisualScripting;
using UnityEngine;

//Weapon Specific Data
[CreateAssetMenu]
public class WeaponData : ItemData
{   
    public int weaponDamage;
    public float attackDuration;
    public float weaponCooldown;

    //Overrides Use() for weapon specific functionality
    public override void Use(ItemInstance itemInstance, InventoryManager inventoryManager)
    {
        inventoryManager.EquipWeapon(itemInstance);
    }
}
