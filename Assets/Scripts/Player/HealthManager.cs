using UnityEngine;
using TMPro;
using System.Collections;

public class HealthManager : MonoBehaviour
{   
    //------------ References ------------
    public GameManager gameManager;
    public GameUIManager gameUIManager;

    //Health variables
    public bool isDead;
    [SerializeField] private int maxHealth = 100; //Maximum health of player
    [SerializeField] private int currentHealth; //Current health of player
    private int healthRegenAmount = 3; //Amount of health to regen per health regen
    private int healthRegenTimer = 3; //# of seconds to wait between health regens

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeHealth(maxHealth); //Set player health to max
        StartCoroutine(HealOverTime());
        isDead = false; 
    }

    //Handles changes to the  health (positive or negative)
    public void ChangeHealth(int amount)
    {   
        if(!isDead) //Stop changing player's health if they are dead
        {
            //Change health by specified amount
            if(currentHealth + amount >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth += amount;
            }

            //If player has no health
            if (currentHealth <= 0)
            {
                currentHealth = 0; 
                Die(); //They are dead
            }

            //Show player's current health
            gameUIManager.UpdateHealthText(currentHealth);
        }
    }

    //Handles death requirements
    void Die()
    {
        isDead = true;
        StopCoroutine(HealOverTime()); //Stops the player healing over time
        //Play death animation, etc. (to be added)
        //Debug.Log("You are dead!");
        gameManager.GameOver();

    }

    //Heals the player over time
    IEnumerator HealOverTime()
    {
        while(true)
        {   
            ChangeHealth(healthRegenAmount);
            yield return new WaitForSeconds(healthRegenTimer);
        }
    }
}
