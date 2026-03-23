using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //------------ References ------------
    public WeaponData weaponData;

    //------------ Private Variables ------------
    private BoxCollider boxCollider;
    private bool canDoDamage = false;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        canDoDamage = false;
    }

    public IEnumerator StartAttackRoutine()
    {
        StartAttack();

        yield return new WaitForSeconds(weaponData.attackDuration);

        EndAttack();
    }

    void StartAttack()
    {
        boxCollider.enabled = true;
        canDoDamage = true;
        transform.localPosition += Vector3.forward / 2;
    }

    void EndAttack()
    {
        canDoDamage = false;
        boxCollider.enabled = false;
        transform.localPosition += Vector3.back / 2;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && canDoDamage)
        {
            canDoDamage = false;
            other.GetComponent<HealthManager>().ChangeHealth(-weaponData.weaponDamage);
            Debug.Log("Doing attack things! - Player Detected");
        } else if (other.CompareTag("Enemy") && canDoDamage)
        {
            canDoDamage = false;
            other.GetComponent<EnemyHealthManager>().ChangeHealth(-weaponData.weaponDamage);
            Debug.Log("Doing attack things! - Enemy Detected");
        }
    }
}
