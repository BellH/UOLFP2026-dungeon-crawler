using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{      

    //------------ Private Variables ------------
    private List<GameObject> enemySpawners = new List<GameObject>();
    private int baseSpawnChance = 5;
    private int spawnChanceModifer = 1;

    //Manages enemy spawns, called by dungeon generator
    public void ManageEnemySpawns()
    {
        //Gets all spawners
        FindAllEnemySpawners();

        //Spawns enemies
        SpawnEnemies();

        //Destroys spawners & clears list
        DestroySpawners();
    }

    //Gets all enemy spawners in scene
    void FindAllEnemySpawners()
    {   
        GameObject[] spawnersFound = GameObject.FindGameObjectsWithTag("EnemySpawner");

        if(spawnersFound != null)
        {
            foreach(GameObject spawner in spawnersFound)
            {
                enemySpawners.Add(spawner);
            }
        }
    }

    //Tells each spawner to spawn an enemy & gives chance value for spawn
    void SpawnEnemies()
    {
        int chance = baseSpawnChance; //Base chance (To be synced to floor/level later)

        //For each spawner
        foreach(GameObject spawner in enemySpawners)
        {
            //Try to spawn enemy
            spawner.GetComponent<EnemySpawner>().SpawnEnemy(chance);

            //If enemy did spawn
            if(spawner.GetComponent<EnemySpawner>().DidEnemySpawn())
            {
                chance -= spawnChanceModifer; //Next spawn is less likely
            } else //If enemy didn't spawn
            {
                chance += spawnChanceModifer; //Next spawn is more likely
            }
        }
    }

    //Tells each spawner to destroy itself
    void DestroySpawners()
    {
        foreach(GameObject spawner in enemySpawners)
        {
            spawner.GetComponent<EnemySpawner>().DestroySpawner();
        }

        enemySpawners.Clear();
    }
}