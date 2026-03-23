using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine;
using Unity.VisualScripting;

//Handles player inventory
public class InventoryManager : MonoBehaviour
{   
    //------------ References ------------
    public GameUIManager gameUIManager;

    //------------ Public Variables ------------
    public ItemInstance equippedWeapon = null;
    public GameObject equippedWeaponPrefab;

    public ItemData starterWeapon;
    public Inventory inventory;

    void Start()
    {
        //Creates a new inventory
        inventory = ScriptableObject.CreateInstance<Inventory>();
        
        //If player starts with weapon (specified in inspector, for testing purposes)
        if(starterWeapon != null)
        {
            AddToInventory(new ItemInstance(starterWeapon)); //Add weapon to inv
            EquipWeapon(inventory.weapons[0]); //And equip it
        }
    }

    //Creates an instance of the selected weapon 
    public void EquipWeapon(ItemInstance weapon)
    {
        //Debug.Log("EquipWeapon function!");
        if(equippedWeapon != null)
        {
            UnequipWeapon();
        }
        equippedWeapon = weapon;
        equippedWeaponPrefab = Instantiate(weapon.itemType.itemPrefab, transform); //Attaches the prefab instance as child of player 
    }

    //Destroys the prefab instance of currently equipped weapon and unequips it
    void UnequipWeapon()
    {
        equippedWeapon = null;
        Destroy(equippedWeaponPrefab);
    }

    //Adds item to appropriate inventory list by category
    public void AddToInventory(ItemInstance item)
    {
        if(item.itemType.category == "Weapons")
        {
            inventory.weapons.Add(item);
        } else if(item.itemType.category == "Consumables")
        {
            inventory.consumables.Add(item);
        }
    }
}
