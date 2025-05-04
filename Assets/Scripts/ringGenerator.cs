using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using JetBrains.Annotations;
using UnityEngine;

public class ringGenerator : MonoBehaviour
{
    public float bpm = 3;
    public GameObject ring;
    public GameObject spawnObj;
    public float growSpeed = 2;
    public int targetSizeValue = 5;
    public float fadeDuration = 1;
    public float ringHeight = 0.2f;
    [SerializeField] Timemanagement timemanagement;

    

    public void StartRingSpawn()
    {
        Invoke(nameof(BeginSpawning), bpm);
        Debug.Log("SpawnRingStarted");
    }

    private void BeginSpawning(){
        StartCoroutine(SpawnRingLoop());
    }

    IEnumerator SpawnRingLoop(){
        while(timemanagement.remainingTime >= 0){
            SpawnRing();
            yield return new WaitForSeconds(bpm);
        }
    }

    void SpawnRing(){
        GameObject newRing = Instantiate(ring, spawnObj.transform.position, Quaternion.Euler(0, Random.Range(0, 361), 0));
        newRing.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        RingBehavior ringScript = newRing.GetComponent<RingBehavior>();
        ringScript.Init(targetSizeValue, growSpeed, fadeDuration);
    }
}
