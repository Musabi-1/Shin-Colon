using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RingCollide : MonoBehaviour
{
    [SerializeField] TowerMovement towerMovement;
    [SerializeField] private LayerMask knightLayer;

    void OnTriggerEnter(Collider ring){
        if(ring.CompareTag("Ring")){
            Debug.Log("Ring detected");
            TriggerNearbyKnights();
        }
    }

    private void TriggerNearbyKnights(){
        Collider[] hits = Physics.OverlapSphere(transform.position, towerMovement.circleRadius, knightLayer);
        foreach(var hit in hits){
            KnightAttack knight = hit.GetComponent<KnightAttack>();
            if(knight != null){
                Debug.Log("Knight found");
                knight.AttackEnemy();
            }
        }
    }

}
