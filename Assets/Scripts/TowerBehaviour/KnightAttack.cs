using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class KnightAttack : MonoBehaviour
{
    [SerializeField] private TowerTargetEnemy towerTargetEnemy;
    [SerializeField] private DetectRange detectRange;
    private int attackDamage;

    private void Start()
    {
        attackDamage = detectRange.attackDamage;
    }

    public void AttackEnemy()
    {
        Transform targetEnemy = towerTargetEnemy.targetEnemy;
        Debug.Log("HitObject: " + targetEnemy.name + "Hitby" + gameObject.name);
        if(targetEnemy.gameObject == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, targetEnemy.position);

        if(distance <= towerTargetEnemy.attackRange)
        {
            EnemyHp enemyHp = targetEnemy.GetComponentInParent<EnemyHp>();
            if(enemyHp != null)
            {
                enemyHp.TakeDamage(attackDamage);
            }
        }
        else
        {
            Debug.Log("Target out of attack range.");
        }
    }
}

