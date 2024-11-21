using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField]
    private int jumpForce = 3;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("JumpPad"))
        {
            Jumped();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("JumpPad"))
        {
            Jumped();
        }
    }

    private void Jumped()
    {
        rigid.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }
}
