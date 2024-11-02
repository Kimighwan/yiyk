using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Transform player;            // �÷��̾� ��ġ
    public float moveDistance = 3f;     // ���� �̵� �Ÿ�
    public float moveSpeed = 1f;        // ���� �⺻ �̵� �ӵ�
    public float approachSpeed = 2f;    // �÷��̾� ���� �� �ӵ�
    public float approachRange = 5f;    // ���� ���� �Ÿ�
    public float approachDuration = 1f;  // ���� ���� �ð�
    private float fixedY;
    private Animator animator;

    private Vector3 startPosition;      // ���� ��ġ
    private bool movingLeft = true;     // �̵� ���� üũ
    private bool isApproaching = false; // �÷��̾� ���� �� ����
    private SpriteRenderer spriteRenderer; // ��������Ʈ ������
    private FadeManager fadeManager;    // ���̵� �Ŵ���
    public Sprite hitSprite;
    private Coroutine currentAnimationCoroutine;

    private int health = 3;             // ���� ü��
    private bool isHit = false;          // �ǰ� ���� üũ

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        fadeManager = FindObjectOfType<FadeManager>(); // ���̵� �Ŵ��� ã��
        animator = GetComponent<Animator>();
        fixedY = transform.position.y;
        currentAnimationCoroutine = StartCoroutine(MovePattern()); // �̵� ���� ����
    }

    void Update()
    {
        if (isHit) return;

        // �÷��̾���� �Ÿ� üũ
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= approachRange && !isApproaching)
        {
            StopAllCoroutines();
            StartCoroutine(ApproachPlayer());
        }

        // ���콺 ���� Ŭ�� ����
        if (Input.GetMouseButtonDown(0) && !isHit)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                TakeDamage(1);
            }
        }

        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
    }

    // �÷��̾�� �浹 ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fadeManager.FadeOutAndRestart(); // ���̵� �ƿ� �� �� �����
        }
    }

    // ���� ���� ó��
    private void TakeDamage(int damage)
    {
        spriteRenderer.sprite = hitSprite;
        Debug.Log("�ǰ� �̹��� ����");
        health -= damage; // ü�� ����
        isHit = true; // �ǰ� ���� ����
        animator.enabled = false; // �ִϸ��̼� ����
        StartCoroutine(HandleHit()); // �ǰ� ó�� ����

        if (health <= 0)
        {
            Destroy(gameObject); // ü���� 0 ������ ��� ������Ʈ �ı�
        }
    }

    IEnumerator HandleHit()
    {
        yield return new WaitForSeconds(2f); // 2�� ���� �ǰ� �̹��� ǥ��
        spriteRenderer.sprite = null; // �ǰ� �̹��� ����
        isHit = false; // �ǰ� ���� ����
        animator.enabled = true; // �ִϸ��̼� �簳
        currentAnimationCoroutine = StartCoroutine(MovePattern()); // �̵� ���� �簳
    }

    IEnumerator MovePattern()
    {
        while (true)
        {
            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(1f);

            // �̵��� ��ġ ����
            float targetX = movingLeft ? startPosition.x - moveDistance : startPosition.x + moveDistance;
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

            // ��ǥ ��ġ���� �̵�
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                animator.SetTrigger("Jump");
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // �̵� ���� ��ȯ
            movingLeft = !movingLeft;
            spriteRenderer.flipX = movingLeft;
        }
    }

    IEnumerator ApproachPlayer()
    {
        isApproaching = true;
        float elapsedTime = 0f;

        while (elapsedTime < approachDuration)
        {
            Vector3 targetPosition = new Vector3(player.position.x, fixedY, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, approachSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            //animator.SetTrigger("Enemy_Jump");
            yield return null;
        }

        
        isApproaching = false;
        StartCoroutine(MovePattern()); // �̵� ���� �簳
    }
}