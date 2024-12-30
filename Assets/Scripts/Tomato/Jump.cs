using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField]
    private int jumpForce = 10;

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

    private void Jumped()
    {
        rigid.velocity = Vector3.zero;  // ������ ���� �� �丶�� �ӵ� 0 ���� // ������ ������ ���ֱ� ����.
        rigid.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        AudioManager.Instance.PlaySFX(SFX.JumpPad);
    }
}
