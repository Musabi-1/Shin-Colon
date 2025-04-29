using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private List<GameObject> gameObjects;
    [SerializeField] private Timemanagement timemanagement;
    [SerializeField] private int maxEnemies = 1;
    [SerializeField] private float baseSpawnInterval = 3f;
    [SerializeField] private float difficultyRamp = 0.05f;
    [SerializeField] private float randomnessFactor = 0.5f;

    private int enemiesSpawned = 0;
    private float elapsedTime = 0f;

    public void StartEnemySpawn(){
        StartCoroutine(EnemySpawnLoop());
    }

    IEnumerator EnemySpawnLoop(){
        while(enemiesSpawned < maxEnemies && timemanagement.remainingTime > 0){
            float difficultyMuiltiplier = 1f + (elapsedTime * difficultyRamp);
            float interval = baseSpawnInterval / difficultyMuiltiplier;

            float randomFactor = Random.Range(1f - randomnessFactor,1f + randomnessFactor);
            float finalInterval = Mathf.Max(0.5f, interval * randomFactor);

            SpawnEnemy();
            enemiesSpawned++;

            yield return new WaitForSeconds(finalInterval);
            elapsedTime += finalInterval;
        }
    }

    private void SpawnEnemy(){
        Instantiate(gameObjects[0], spawnPoint.transform.position, Quaternion.identity);
    }
}
