using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private int attackDamage = 50;
    [SerializeField] private float attackRadius = 4f;
    private bool isAttacking = false;
    [SerializeField] private int hp = 30;
    private GameObject castleParent;

    private void Start()
    {
        castleParent = gameObject.transform.parent.gameObject;
    }  

    public IEnumerator AttackEnemy(){
        if(!isAttacking){
            isAttacking = true;
            yield return new WaitForSeconds(attackSpeed);
            DamageEnemy();
            isAttacking = false;
        }

    }

    private void DamageEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRadius);

        foreach (Collider col in enemiesInRange)
        {
            if(col.CompareTag("Enemy")){
                EnemyAttack enemyAttack = col.GetComponentInParent<EnemyAttack>();
                if(enemyAttack != null){
                    enemyAttack.takeDamage(attackDamage);
                    Debug.Log("Damage dealt in enemy");
                }
            }
        }
    }

    public void TakeDamage(int damage){
        hp -= damage;
        if(hp <= 0){
            Die();
        }
    }

    private void Die()
    {
        Destroy(castleParent);
    }
}
