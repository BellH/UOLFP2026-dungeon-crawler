using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{   
    //Health variables
    private int maxHealth = 20; //Maximum health of enemy
    [SerializeField] private int currentHealth; //Current health of enemy
    public bool isDead;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeHealth(maxHealth);
        isDead = false;
    }

    //Handles changes to the  health (positive or negative)
    public void ChangeHealth(int amount)
    {   
        if (currentHealth + amount >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += amount;
        }

        //Check if the enemy has died
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //Handles death requirements
    void Die()
    {
        isDead = true;

        //Play death animation, etc. (to be added)
        //When death animation finishes (to be added)
        //Puff of smoke (particle system) (to be added)

        //Get rid of the gameObject
        if(gameObject != null)
        {
            Destroy(gameObject);
            //Debug.Log("Enemy is dead!");
        }
    }
}
