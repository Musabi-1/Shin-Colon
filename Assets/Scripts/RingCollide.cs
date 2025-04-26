using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RingCollide : MonoBehaviour
{
    public float activatedSeconds = 1;
    private Color basecolour;
    private Renderer tower;
    private bool activated = false;
    
    void Start(){
        tower = GetComponent<Renderer>();
        basecolour = tower.material.color;
    }

    void OnTriggerEnter(Collider ring){
        if(ring.CompareTag("Ring") && !activated){
            activated = true;
            StartCoroutine(Activate());
            Debug.Log("Colliding with Ring");
            activated = false;
        }
    }

    IEnumerator Activate(){
        tower.material.color = Color.red;

        yield return new WaitForSeconds(activatedSeconds);

        tower.material.color = basecolour;
    }
}
