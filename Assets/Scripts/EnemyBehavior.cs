using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject castle;
    public float attackRange;
    public float attackSpeed;
    public int attackDamage = 5;
    public float moveSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        if(Vector3.Distance(this.transform.position, castle.transform.position) <= attackRange){
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack(){
        while(true){
        }
    }

    public void onAttack(int hp){
        hp = hp - attackDamage;
    }

    
}
