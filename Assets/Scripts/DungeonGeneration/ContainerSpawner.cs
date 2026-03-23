using UnityEngine;

public class ContainerSpawner : MonoBehaviour
{
    public GameObject containerPrefab;
    private bool containerDidSpawn;

    //Instantiates a container at spawner location
    public void SpawnContainer(int chance) 
    {
        if(chance > Random.Range(0, 10))
        {
            Instantiate(containerPrefab, transform.position, transform.rotation);
            containerDidSpawn = true;
        }
        containerDidSpawn = false;
    }

    //Tells container spawn manager if spawn was successful
    public bool DidContainerSpawn()
    {
        return containerDidSpawn;
    }

    //Destroys spawner
    public void DestroySpawner()
    {
        Destroy(gameObject);
    }
}
