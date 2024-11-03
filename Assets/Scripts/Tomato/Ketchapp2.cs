using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ketchapp2 : MonoBehaviour
{
    Animation anim;
    WaitForSeconds wait = new WaitForSeconds(2.7f);
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        anim.Play();
    }

    IEnumerator Delete()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 2.7f)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / 2.7f);
            spriteRenderer.color = new Color(1, 1, 1, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
