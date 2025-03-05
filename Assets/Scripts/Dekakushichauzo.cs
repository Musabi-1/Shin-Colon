using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kyuutai : MonoBehaviour
{
    public float timer = 0f;
    public float growTime = 5f;
    public float maxSize = 2f;
    public bool isMaxSize = false;
    void Start(){
        if(isMaxSize == false){
            StartCoroutine(Grow());
        }
    }
    private IEnumerator Grow(){
        Vector3 startScale = transform.localScale;
        Vector3 maxScale = new Vector3(maxSize,maxSize,maxSize);

        do{
            transform.localScale = Vector3.Lerp(startScale,maxScale, timer /growTime);
            timer += Time.deltaTime;
            yield return null;
        }
        while(timer <growTime);
        isMaxSize = true;
    }
}
