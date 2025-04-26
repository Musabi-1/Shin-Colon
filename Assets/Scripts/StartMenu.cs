using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartMenu : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject Timer;
    void Awake()
    {
        Time.timeScale = 0;
        startMenu.SetActive(true);
        Timer.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Time.timeScale = 1;
        startMenu.SetActive(false);
        Timer.SetActive(true);
    }
}   
