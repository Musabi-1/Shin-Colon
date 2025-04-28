using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timemanagement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] pausemenu Pausemenu;
    public float remainingTime;
    void Update()
    {
        if(remainingTime > 0){
        remainingTime -= Time.deltaTime;
        }
        else if(remainingTime <0){
            remainingTime = 0;
            Pausemenu.GameComplete();
        }
        
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
