using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHp : MonoBehaviour
{
     [SerializeField] private int hp = 30;
    private GameObject tower;

    private void Start()
    {
        tower = gameObject;
    }

    public void TakeDamage(int damage){
        hp -= damage;
        if(hp <= 0){
            Die();
        }
    }

    private void Die(){
        Destroy(tower);
    }

}
