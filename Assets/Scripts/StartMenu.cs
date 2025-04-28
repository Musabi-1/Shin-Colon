using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartMenu : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject Timer;
    [SerializeField] private ringGenerator RingGenerator;
    [SerializeField] private EnemySpawner enemySpawner;
    void Awake()
    {
        Time.timeScale = 0;
        startMenu.SetActive(true);
        Timer.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameStart();
    }

    private void GameStart(){
        Time.timeScale = 1;
        startMenu.SetActive(false);
        Timer.SetActive(true);
        RingGenerator.StartRingSpawn();
        enemySpawner.StartEnemySpawn();
    }
}   
