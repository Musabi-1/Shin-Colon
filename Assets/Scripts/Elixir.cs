using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Elixir : MonoBehaviour
{
    [HideInInspector] public int elixirCount;
<<<<<<< Updated upstream
    [SerializeField] private ringGenerator RingGenerator;
=======
>>>>>>> Stashed changes
    [SerializeField] private int Maxelixir;
    [SerializeField] private List<Image> elixirImages;
    [SerializeField] private ringGenerator RingGenarator;
    private float elixirSeconds;
    private float elixirTimer;
    private float elixirSeconds;
    private void Awake()
    {
        elixirSeconds = RingGenerator.bpm;
        elixirCount = 0;
        elixirTimer = 0f;
        elixirSeconds = RingGenarator.bpm;

        foreach(var img in elixirImages){
            img.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if(elixirCount >= Maxelixir){
            return;
        }
        elixirTimer += Time.deltaTime;
        if(elixirTimer >= elixirSeconds){
            elixirTimer = 0f;
            AddElixir();
        }
    }

    private void AddElixir(){
        if(elixirCount < elixirImages.Count){
            elixirImages[elixirCount].gameObject.SetActive(true);
        }
        elixirCount++;
    }

    public void RefundElixir(int cost){
        elixirCount += cost/2;
        if(elixirCount > Maxelixir){
            elixirCount = Maxelixir;
        }
        ElixirVisuals();
    }

    public void RemoveElixir(int cost){
        elixirCount -= cost;
        elixirCount = Mathf.Max(elixirCount, 0);

        ElixirVisuals();
    }

    private void ElixirVisuals(){
        for(int i = 0; i < elixirImages.Count; i++){
            elixirImages[i].gameObject.SetActive(i<elixirCount);
        }
    }
}
