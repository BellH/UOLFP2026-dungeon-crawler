using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // ------------ References ------------
    //Other GameObjects
    public GameObject player;

    //Self
    //public EnemyHealthManager enemyHealthManager;
    public EnemyCombatController enemyCombatController;

    //------------ Private Variables ------------
    private float perceptionDist = 5f; //Distance at which enemy can 'see' player (and will chase them)
    private float attackRange = 2;

    //Movement Variables
    private float enemyMoveSpeed = 3f;
    private float enemyMoveSpeedModifier; //Multiplied by enemyMoveSpeed for dynamic speeds
    private float passiveSpeedModifier = 0.5f; //Enemy moves slower when passive
    private float chaseSpeedModifer = 1.0f; //Full speed when chasing
    private Vector3 movementDirection;
    bool isPassive;

    void Start()
    {
        player = GameObject.Find("Player");
        isPassive = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if(CalcDistFromPlayer() <= attackRange) //If within attack range
        {
            enemyCombatController.Attack(); //Attack
        } else //Else
        {
            EnemyMovementController(); //Move
        }
    }

    //Handles Enemy Movement functions & transitions between them (brains of the enemy AI)
    void EnemyMovementController()
    {   
        //Enemy is entering passive state 
        if (!isPassive && CalcDistFromPlayer() > perceptionDist) //!isPassive prevents the coroutine being continuously started 
        {   
            isPassive = true;
            enemyMoveSpeedModifier = passiveSpeedModifier;
            StartCoroutine(GeneratePassiveDirection());   
        }

        //Enemy is entering chasing state
        if(CalcDistFromPlayer() < perceptionDist)
        {
            isPassive = false; 
            enemyMoveSpeedModifier = chaseSpeedModifer;
            StopCoroutine(GeneratePassiveDirection());
            GenerateChaseDirection();
        }

        EnemyMovement();
    }

    //Finds enemy's distance from the player
    float CalcDistFromPlayer()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        return dist;
    }

    //Moves the enemy based on the current movement direction
    void EnemyMovement()
    {
        transform.Translate(enemyMoveSpeed * enemyMoveSpeedModifier * Time.deltaTime * movementDirection);
    }

    //------------ Movement Direction Functions ------------
    //Generates random enemy movement (patrolling)
    IEnumerator GeneratePassiveDirection()
    {   
        while(isPassive)
        {
            movementDirection = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2));
            yield return new WaitForSeconds(5f);
        }
    }

    //Generates the chase direction (enemy -> player)
    void GenerateChaseDirection()
    {
        transform.LookAt(player.transform);
        movementDirection = Vector3.forward;
    }
}
