using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float maxSpeed;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Stop  Speed?
        if (Input.GetButtonUp("Horizontal"))
        {
            float x = Input.GetAxis("Horizontal");
            rigid.AddForce(new Vector2(x, rigid.velocity.y));
        }
            //rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);

        // Direction Sprite // �丶�� ��,�ڰ� ��������?
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.2)
            anim.SetBool("isMove", false);
        else
            anim.SetBool("isMove", true);
    }

    private void FixedUpdate()
    {
        // Move By Keyboard
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // Max Speed
        if (rigid.velocity.x > maxSpeed) // Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // Left Max Speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
    }
}
