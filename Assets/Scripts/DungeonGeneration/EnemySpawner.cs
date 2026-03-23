using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    private bool enemyDidSpawn;

    //Instantiates an enemy at spawner location
    public void SpawnEnemy(int chance) 
    {
        if(chance > Random.Range(0, 10))
        {
            Instantiate(EnemyPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            enemyDidSpawn = true;
        }
        enemyDidSpawn = false;
    }

    //Tells enemy spawn manager if spawn was successful
    public bool DidEnemySpawn()
    {
        return enemyDidSpawn;
    }

    //Destroys spawner 
    public void DestroySpawner()
    {
        Destroy(gameObject);
    }
}
