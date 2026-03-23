using System.Collections;
using UnityEngine;

public class DamagingArea : MonoBehaviour
{
    int damageAmount = 20; //Amount of damage to apply to player when they enter area
    float damageSpeed = 1f; //Time to wait in between damaging player
    bool isInArea;

    //Detects object entering damaging area
    void OnTriggerEnter(Collider other)
    {
        isInArea = true;
        StartCoroutine(ApplyDamageOverTime(other));
    }

    //Detects object leaving damaging area
    void OnTriggerExit(Collider other)
    {
        isInArea = false;
        StopCoroutine(ApplyDamageOverTime(other));
    }

    //Applies damge over time
    IEnumerator ApplyDamageOverTime(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            while (isInArea)
            {
                other.GetComponent<HealthManager>().ChangeHealth(-damageAmount);
                yield return new WaitForSeconds(damageSpeed); //Wait before applying damage again (prevents instadeath)
            }   
        } else if (other.CompareTag("Enemy"))
        {
            while (isInArea)
            {
                other.GetComponent<EnemyHealthManager>().ChangeHealth(-damageAmount);
                yield return new WaitForSeconds(damageSpeed); //Wait before applying damage again (prevents instadeath)
            }   
        }

    }
}
