using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackSpeed = 2f;
    [SerializeField] private EnemyPathing enemyPathing;
    private CastleBehavior castleBehavior;
    private bool wasAtTarget = false;
    private bool wasAtTower = false;
    private Coroutine attackCoroutine;
    [SerializeField] private float attackRange = 2f;
    private bool canAttackTower = false;

    private void Update()
    {
        if(castleBehavior != null && castleBehavior.gameObject == null){
            HandleDeadCastle();
            return;
        }

        GameObject closestTower = FindClosestTowerInRange();

        if(canAttackTower){
            if(!wasAtTower){
                enemyPathing.StopMoving();
                attackCoroutine = StartCoroutine(AttackTower(closestTower));
                wasAtTower = true;
            }
        }
        else if(enemyPathing.targetObject != null){
            if(castleBehavior == null || castleBehavior.gameObject != enemyPathing.targetObject){
                castleBehavior = enemyPathing.targetObject.GetComponent<CastleBehavior>();
            }

            if(Vector3.Distance(transform.position, enemyPathing.targetObject.transform.position) <= attackRange){
                enemyPathing.StopMoving();
                if(!wasAtTarget){
                    attackCoroutine = StartCoroutine(AttackCastle());
                    wasAtTarget = true;
                }
            }
        }
        if(wasAtTower && (closestTower == null || Vector3.Distance(transform.position, closestTower.transform.position) > attackRange)){
            HandleStopAttack();
        }
    }

    IEnumerator AttackCastle(){
        while(castleBehavior != null && castleBehavior.gameObject!= null){
            castleBehavior.takeDamage(attackDamage);
            yield return new WaitForSeconds(attackSpeed);
        }

        HandleStopAttack();
    }

    IEnumerator AttackTower(GameObject tower){
        TowerHp towerAttack = tower.GetComponent<TowerHp>();
        while(towerAttack != null && towerAttack.gameObject != null){
            towerAttack.TakeDamage(attackDamage);
            yield return new WaitForSeconds(attackSpeed);
        }

        HandleStopAttack();
    }

    private GameObject FindClosestTowerInRange(){
        GameObject[] towers = GameObject.FindGameObjectsWithTag("TargettableTower");
        GameObject closestTower = null;
        float closestDistance = Mathf.Infinity;

        foreach(GameObject tower in towers){
            Transform towerTransform = tower.transform;
            float distance = Vector3.Distance(transform.position, towerTransform.position);
            if(distance < closestDistance){
                closestDistance = distance;
                closestTower = tower;
            }
        }
        if(closestDistance <= attackRange){
            canAttackTower = true;
        }
        else{
            canAttackTower = false;
        }
        return closestTower;
    }

    private void HandleDeadCastle(){
        enemyPathing.isAtTarget = false;
        HandleStopAttack();
    }

    private void HandleStopAttack(){
        if(attackCoroutine != null){
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        wasAtTarget =  false;
        wasAtTower = false;
        enemyPathing.ResumeMovement();
    }
}
