using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CastleBehavior : MonoBehaviour
{
    [SerializeField] private int hp = 6;
    private GameObject castle;
    [SerializeField] Timemanagement timemanagement;

    public void Start()
    {
        castle = transform.parent.gameObject;
    }

    public void takeDamage(int damage){
        hp-= damage;
        Debug.Log("Castle Damaged");
        if(hp <=0){
            Die();
        }
    }

    private void Die(){
        Destroy(castle);
    }
}
