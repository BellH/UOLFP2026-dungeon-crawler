using System.Collections.Generic;
using UnityEngine;

public class ContainerSpawnManager : MonoBehaviour
{
    // //------------ Private Variables ------------
    private List<GameObject> containerSpawners = new List<GameObject>();
    private int baseSpawnChance = 5;
    private int spawnChanceModifer = 1;

    //Manages container spawns, called by dungeon generator
    public void ManageContainerSpawns()
    {
        //Gets all spawners
        FindAllContainerSpawners();

        //Spawns container
        SpawnContainer();

        //Destroys spawners & clears list
        DestroySpawners();
    }

    //Gets all container spawners in scene
    void FindAllContainerSpawners()
    {   
        GameObject[] spawnersFound = GameObject.FindGameObjectsWithTag("ContainerSpawner");
        
        if(spawnersFound != null)
        {
            foreach(GameObject spawner in spawnersFound)
            {
                containerSpawners.Add(spawner);
            }
        }
    }

    //Tells each spawner to spawn a container & gives chance value for spawn
    void SpawnContainer()
    {
        int chance = baseSpawnChance; //Base chance (To be synced to floor/level later)

        //For each spawner
        foreach(GameObject spawner in containerSpawners)
        {
            //Try to spawn container
            spawner.GetComponent<ContainerSpawner>().SpawnContainer(chance);

            //If container did spawn
            if(spawner.GetComponent<ContainerSpawner>().DidContainerSpawn())
            {
                chance -= spawnChanceModifer; //Next spawn is less likely
            } else //If container didn't spawn
            {
                chance += spawnChanceModifer; //Next spawn is more likely
            }
        }
    }

    //Tells each spawner to destroy itself
    void DestroySpawners()
    {
        foreach(GameObject spawner in containerSpawners)
        {
            spawner.GetComponent<ContainerSpawner>().DestroySpawner();
        }

        containerSpawners.Clear();
    }
}
