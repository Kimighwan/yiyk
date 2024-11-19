using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField]
    private int jumpForce = 5;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("JumpPad"))
        {
            Jump();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("JumpPad"))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rigid.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }
}
