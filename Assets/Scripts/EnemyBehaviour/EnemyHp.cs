using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHp : MonoBehaviour
{
    [SerializeField] private int maxHp = 100;
    [SerializeField] private Image healthBar;
    [SerializeField] private int hp;

    private void Start()
    {
        hp = maxHp;
    }

    public void TakeDamage(int damage){
        hp -= damage;
        healthBar.fillAmount = (float)hp / maxHp;
        if(hp <= 0){
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
