using UnityEngine;
using UnityEngine.Timeline;

public class CombatController : MonoBehaviour
{   
    //------------ References ------------
    public InventoryManager inventoryManager;

    //------------Private Variables ------------
    private GameObject weaponPrefab;
    private WeaponData weaponData;
    private float attackCooldownTimer;

    void Update()
    {
        if(attackCooldownTimer > 0) 
        {
            attackCooldownTimer -= Time.deltaTime; //Decrements cooldown timer every second
        }
    }

    public void Attack()
    {
        if(attackCooldownTimer <= 0) //if attack has cooled down (prevents instakill)
        {   
            //Gets currently equipped weapon information
            if(inventoryManager.equippedWeapon != null)
            {
                weaponPrefab = inventoryManager.equippedWeaponPrefab;
                weaponData = inventoryManager.equippedWeapon.itemType as WeaponData; //Cast item data as weapon data

                AddToCooldownTimer(weaponData.weaponCooldown + weaponData.attackDuration); //Start attack cooldown timer
                StartCoroutine(weaponPrefab.GetComponent<WeaponController>().StartAttackRoutine());
                //Debug.Log("CombatController Attack() method!");
            }
        }
    }

    //Adds to attack cooldown timer

    void AddToCooldownTimer(float time)
    {
        attackCooldownTimer += time;
    }
}
