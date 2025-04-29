using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRange : MonoBehaviour
{
    public int attackDamage;
    public event Action<Collider> OnEnemyEnter;
    public event Action<Collider> OnEnemyExit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy")){
            OnEnemyEnter?.Invoke(other);
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy")){
            OnEnemyExit?.Invoke(other);
        }
    }
}
