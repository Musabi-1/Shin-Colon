using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBehavior : MonoBehaviour
{   private float growSpeed;
    private int targetSizeValue;
    private Vector3 targetSize;
    private SpriteRenderer ringSprite;
    private float fadeDuration;
    private bool isInitialised = false;


    public void Init(int targetSizeValue, float growSpeed, float fadeDuration){
        this.targetSize = new Vector3(targetSizeValue,targetSizeValue,targetSizeValue);
        this.growSpeed = growSpeed;
        this.fadeDuration = fadeDuration;
        ringSprite = GetComponent<SpriteRenderer>();
        isInitialised = true;
    }

    void Update()
    {
        if(!isInitialised) return;

        transform.localScale = Vector3.Lerp(transform.localScale, targetSize, Time.deltaTime * growSpeed);
        if(Mathf.Abs(transform.localScale.x - targetSizeValue) < 0.5f && !fading){
            StartCoroutine(fadeOut(ringSprite, fadeDuration));
            fading = true;
        }
    }

    private bool fading = false;

     IEnumerator fadeOut(SpriteRenderer MyRenderer, float duration){
        float counter = 0;
        Color spriteColor = MyRenderer.color;

        while(counter < duration){
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / duration);

            MyRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }

        Destroy(MyRenderer.gameObject);
    }

}
