using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private float attackSpeed = 2f;
    [SerializeField] private int attackDamage = 50;
    [SerializeField] private float attackRange = 4f;
    private bool canAttack = false;
    Collider targetEnemy = null;
    private Coroutine attackCoroutine;

    private void Update()
    {
        RangeChecker();
        if(canAttack && attackCoroutine == null){
            attackCoroutine = StartCoroutine(AttackEnemy());
        }
        else if(!canAttack && attackCoroutine != null){
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    public IEnumerator AttackEnemy(){
        while(canAttack){
            if(targetEnemy != null){
                DamageEnemy();
            }
            yield return new WaitForSeconds(attackSpeed);
        }
    }

    private void DamageEnemy()
    {
        EnemyHp enemyAttack = targetEnemy.GetComponentInParent<EnemyHp>();
        if(enemyAttack != null){
            enemyAttack.TakeDamage(attackDamage);
        }
    }

    private void RangeChecker(){

        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange);
        float closestDistance = Mathf.Infinity;
        Collider closestEnemy = null;
        foreach (Collider col in enemiesInRange)
        {
            if(col.CompareTag("Enemy")){
                float distance = Vector3.Distance(gameObject.transform.position, col.transform.position);
                if(distance < closestDistance){
                    closestDistance = distance;
                    closestEnemy = col;
                }
            }
        }
        if(closestEnemy != null){
            targetEnemy = closestEnemy;
            canAttack = true;
        }
        else{
            targetEnemy = null;
            canAttack = false;
        }
    }
}
