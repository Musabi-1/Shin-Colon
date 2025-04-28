using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private float spawnInterval = 4;
    [SerializeField] private List<GameObject> gameObjects;
    [SerializeField] private Timemanagement timemanagement;
    [SerializeField] private int maxEnemies = 1;

    public void StartEnemySpawn(){
        Invoke(nameof(BeginenemySpawner), spawnInterval);
    }

    private void BeginenemySpawner(){
        StartCoroutine(enemySpawner());
    }

    IEnumerator enemySpawner(){
        while(maxEnemies > 0){
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
            maxEnemies--;
        }
    }
    private void SpawnEnemy(){
        Instantiate(gameObjects[0], spawnPoint.transform.position, Quaternion.identity);
    }
}
