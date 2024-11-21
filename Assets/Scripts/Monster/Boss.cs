using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigid2D;
    private Vector2 pos;
    private bool isCanDamaged = false;
    private int amountOfBone = 0;
    private int currentDirection = -1; // 1: ������, -1: ����

    public GameObject player;
    public GameObject throwingBone;
    public float speedx = 3f;
    public int maxScale = 5;
    private int nextPattern = 0;

    private static readonly int NONE = 0;
    private static readonly int RUSH = 1;
    private static readonly int THROW = 2;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        StartCoroutine("Rush");
    }

    void lookPlayer()
    {
        float scale = transform.localScale.z;
        transform.localScale = new Vector3(currentDirection * -1 * scale, scale, scale); //DIRECTION ������ �̿��ؼ� �÷��̾����� �ٶ󺸵��� �Ѵ�.
    }

    IEnumerator Rush()
    {
        // �÷��̾���� �浹�� �����ϵ��� ����
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Collider2D bossCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(bossCollider, playerCollider, true); // �浹 ����

        animator.SetTrigger("ready");
        lookPlayer(); // �÷��̾ �ٶ󺸰� ����
        yield return new WaitForSeconds(1); // ��� �غ� �ð�

        animator.SetTrigger("IsDash"); // ��� �ִϸ��̼� ����
        bool isBroken = false;

        // �� ���̾� ����ũ
        int wallLayerMask = LayerMask.GetMask("Wall");
        float rayLength = 5f;

        while (!isBroken)
        {
            yield return new WaitForSeconds(0.1f);

            if (speedx > rigid2D.velocity.x * currentDirection)
            {
                rigid2D.AddForce(transform.right * currentDirection * 1000);
            }

            Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y + 0.5f);
            Debug.DrawRay(rayOrigin, transform.right * currentDirection * rayLength, Color.red, 1f);

            // ������ �浹 ����
            RaycastHit2D rayHit = Physics2D.Raycast(rayOrigin, transform.right * currentDirection, rayLength, wallLayerMask);
            if (rayHit.collider != null)
            {
                // ���� �浹�ϸ� ��ø� ���߰� ���� �������� �̵�
                Debug.Log($"�ε��� ��: {rayHit.collider.name}");
                rigid2D.velocity = Vector2.zero;
                isBroken = true;
            }
        }

        // ��ð� ���� ��, �÷��̾���� �浹�� ����
        Physics2D.IgnoreCollision(bossCollider, playerCollider, false); // �浹 ����

        animator.SetTrigger("IsStunned"); // ���� �ִϸ��̼� ����
        yield return StartCoroutine(stunCounter()); // ���� ī���� ����
        ToggleDirection(); // ���� ��ȯ
        nextPattern = THROW; // ������ �������� ����
        yield return new WaitForSeconds(3);

        nextPatternPlay(); // ���� ���� ����
    }

    IEnumerator Throw()
    {
        lookPlayer();
        animator.SetTrigger("IsThrow");
        yield return new WaitForSeconds(1.5f);

        // ���� ������ ������ �÷��̾ ���� ������
        for (int i = 0; i < 5; i++) //���� ����ŭ ���� ��ȯ�ϰ� ���� ������ ������ �����
        {
            GameObject go = Instantiate(throwingBone, new Vector3(transform.position.x + 3 * currentDirection, transform.position.y, transform.position.z), Quaternion.Euler(0f, 20f, 0f)) as GameObject;
            Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
            go.GetComponent<Rigidbody2D>().velocity = directionToPlayer * 15f; // �÷��̾� �������� ������
        }

        amountOfBone = 0;

        if (0 == Random.Range(0, 2)) nextPattern = RUSH; //���� ������ �������� �����ش�.

        yield return new WaitForSeconds(3);

        nextPatternPlay();
    }

    IEnumerator stunCounter() //������ ȣ��Ǵ� �Լ��� 4�ʰ� ���� �Ŀ� stunned�� false�� �����Ѵ�.
    {
        yield return new WaitForSeconds(4);
        animator.SetTrigger("IsIdle");
    }

    void ToggleDirection()
    {
        currentDirection *= -1; // ���� ��ȯ
    }

    void nextPatternPlay()
    {
        switch (nextPattern)
        {
            case 1:
                StartCoroutine(Rush());
                break;
            case 2:
                StartCoroutine(Throw());
                break;
        }
    }
}
