using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{   
    //------------ References ------------
    public GameObject weaponPrefab;
    public WeaponData weaponData;
    public LootPool enemyLootPool; //Pool of weapons enemy can pick from

    //------------Private Variables ------------
    private float attackCooldownTimer;

    void Start()
    {
        PickRandomWeapon();
        weaponPrefab = Instantiate(weaponPrefab, transform); 
    }

    void Update()
    {
        if(attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime; //Decrements cooldown timer every second
        }
    }

    //Picks a random weapon for enemy to use
    void PickRandomWeapon()
    {
        int itemIndex;
        itemIndex = Random.Range(0, enemyLootPool.loot.Count); //Pick random item index
        weaponData = enemyLootPool.loot[itemIndex] as WeaponData;
        weaponPrefab = weaponData.itemPrefab;
    
    }

    //Tells WeaponController to start weapon's attack
    public void Attack()
    {
        if(attackCooldownTimer <= 0) //if attack has cooled down (prevents instakill)
        {   
            AddToCooldownTimer(weaponData.weaponCooldown + weaponData.attackDuration); //Start attack cooldown timer
            StartCoroutine(weaponPrefab.GetComponent<WeaponController>().StartAttackRoutine()); //Start attack
            //Debug.Log("EnemyCombatController Attack!!");
        }
    }

    //Adds to attack cooldown timer
    public void AddToCooldownTimer(float time)
    {
        attackCooldownTimer += time;
    }
    
}
