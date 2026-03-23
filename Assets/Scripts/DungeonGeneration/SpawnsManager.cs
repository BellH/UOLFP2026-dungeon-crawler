using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnsManager : MonoBehaviour
{
    private EnemySpawnManager enemySpawnManager;
    private ContainerSpawnManager containerSpawnManager;

    //Tells all spawn managers to execute
    public void HandleAllSpawners()
    {
        enemySpawnManager = GetComponent<EnemySpawnManager>();
        containerSpawnManager = GetComponent<ContainerSpawnManager>();

        enemySpawnManager.ManageEnemySpawns();
        containerSpawnManager.ManageContainerSpawns();
    }

    //Finds and destroys all spawner-spawned gameobjects left in scene (enemies & loot containers)
    public void ClearAllSpawns()
    {
        GameObject[] enemySpawns = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject [] containerSpawns = GameObject.FindGameObjectsWithTag("Container");
        
        if(enemySpawns != null)
        {
            foreach(GameObject enemy in enemySpawns)
            {
                //Debug.Log(enemy.name);
                Destroy(enemy);
            }
        }

        if(containerSpawns != null)
        {
            foreach(GameObject container in containerSpawns)
            {
                //Debug.Log(container.name);
                Destroy(container);
            }
        }
    }

}
