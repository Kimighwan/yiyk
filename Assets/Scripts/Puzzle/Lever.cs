using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IPuzzle
{
    public GameObject door;

    private SpriteRenderer spriteRenderer;
    private Animation doorAnim;
    private bool alreadyOpen = false;

    private void Awake()
    {
        doorAnim = door.GetComponent<Animation>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Open()
    {
        if (alreadyOpen)
            return;

        doorAnim.Play();
        alreadyOpen = true;
    }

    public void On()
    {
        spriteRenderer.flipX = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D 콜라이더 닿음");
        if (collision.CompareTag("Line"))
        {
            On();
            Open();
        }
    }

}
