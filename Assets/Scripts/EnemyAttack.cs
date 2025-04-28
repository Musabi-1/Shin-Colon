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
    [SerializeField] private int hp = 100;
    [SerializeField] private float attackRange = 2f;

    private void Update()
    {
        if(castleBehavior != null && castleBehavior.gameObject == null){
            HandleDeadCastle();
            return;
        }

        GameObject closestTower = FindClosestTower();

        if(closestTower != null){
            float distanceToTower = Vector3.Distance(transform.position, closestTower.transform.position);
            Debug.Log("Distance to tower: " + distanceToTower);
            if(distanceToTower <= attackRange){
                Debug.Log("Inrange");
                if(!wasAtTower){
                   enemyPathing.StopMoving();
                    attackCoroutine = StartCoroutine(AttackTower(closestTower));
                    wasAtTower = true;
                }
            }
            else{
                Debug.Log("Tower is out of range distance: " + distanceToTower);
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
            Debug.Log("Attacked");
            castleBehavior.takeDamage(attackDamage);
            yield return new WaitForSeconds(attackSpeed);
        }

        HandleStopAttack();
    }

    IEnumerator AttackTower(GameObject tower){
        TowerAttack towerAttack = tower.GetComponent<TowerAttack>();
        while(towerAttack != null && towerAttack.gameObject != null){
            towerAttack.TakeDamage(attackDamage);
            yield return new WaitForSeconds(attackSpeed);
        }

        HandleStopAttack();
    }

    public void takeDamage(int damage){
        hp -= damage;
        if(hp <= 0){
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    private GameObject FindClosestTower(){
        GameObject[] towers = GameObject.FindGameObjectsWithTag("TargettableTower");
        GameObject closestTower = null;
        float closestDistance = Mathf.Infinity;

        foreach(GameObject tower in towers){
            Transform towerTransform = tower.transform;
            Transform[] children = towerTransform.GetComponentsInChildren<Transform>();

            foreach(Transform child in children){
                if(child.gameObject.CompareTag("TargettableTower")){

                    float distance = Vector3.Distance(transform.position, child.position);

                    if(distance < closestDistance){
                        closestDistance = distance;
                        closestTower = child.gameObject;
                    }
                }
            }
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
