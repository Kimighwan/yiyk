using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pasta : MonoBehaviour
{
    public Transform player;            // �÷��̾� ��ġ
    public float moveDistance = 3f;     // ���� �̵� �Ÿ�
    public float moveSpeed = 1f;        // ���� �⺻ �̵� �ӵ�
    public float approachSpeed = 2f;    // �÷��̾� ���� �� �ӵ�
    public float approachRange = 5f;    // ���� ���� �Ÿ�
    private float fixedY;
    private Animator animator;

    private Vector3 startPosition;      // ���� ��ġ
    private bool movingLeft = true;     // �̵� ���� üũ
    private bool isApproaching = false; // �÷��̾� ���� �� ����
    private SpriteRenderer spriteRenderer; // ��������Ʈ ������
    private FadeManager fadeManager;    // ���̵� �Ŵ���
    public Sprite hitSprite;
    private Sprite originalSprite;      // ���� ��������Ʈ ����
    private Coroutine currentAnimationCoroutine;

    private int health = 10;             // ���� ü��
    private bool isHit = false;         // �ǰ� ���� üũ
    private string currentAnimState;     // ���� �ִϸ��̼� ���� Ȯ���� ���� ����

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        fadeManager = FindObjectOfType<FadeManager>();
        animator = GetComponent<Animator>();
        currentAnimationCoroutine = StartCoroutine(MovePattern());
        currentAnimState = "";
    }

    void Update()
    {
        if (isHit || isApproaching) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= approachRange && !isApproaching)
        {
            if (currentAnimationCoroutine != null)
            {
                StopCoroutine(currentAnimationCoroutine);
                currentAnimationCoroutine = null;
            }

            StartCoroutine(ApproachPlayer());
        }
        else
        {
            if (!isApproaching && currentAnimationCoroutine == null)
            {
                currentAnimationCoroutine = StartCoroutine(MovePattern());
            }
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (isHit)
            {
                StopCoroutine(HandleHit());
                spriteRenderer.sprite = originalSprite;
                isHit = false;
                animator.enabled = true;
            }

            Die();
            return;
        }
        if (isHit) return;

        isHit = true;
        spriteRenderer.sprite = hitSprite;
        StopAllCoroutines();
        animator.enabled = false;
        StartCoroutine(HandleHit());
    }

    IEnumerator HandleHit()
    {
        yield return new WaitForSeconds(2f);

        spriteRenderer.sprite = originalSprite;
        isHit = false;
        animator.enabled = true;
        currentAnimationCoroutine = StartCoroutine(MovePattern());
    }

    IEnumerator MovePattern()
    {

        while (!isApproaching)
        {
            float targetX = movingLeft ? startPosition.x - 5f : startPosition.x + 5f;

            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);


            float elapsedTime = 0f;
            while (elapsedTime < 3f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            movingLeft = !movingLeft;
            spriteRenderer.flipX = movingLeft;

            animator.SetBool("IsFly", false);
            animator.SetBool("IsIdle", true);
            yield return new WaitForSeconds(2f);

            animator.SetBool("IsFly", true);
            animator.SetBool("IsIdle", false);
        }
    }

    IEnumerator ApproachPlayer()
    {
        isApproaching = true;
        animator.SetBool("IsFly", true);
        animator.SetBool("IsIdle", false);

        Vector3 direction = (player.position - transform.position).normalized;

        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
            movingLeft = true;
        }
        else
        {
            spriteRenderer.flipX = false;
            movingLeft = false;
        }

        while (Vector3.Distance(transform.position, player.position) > 0.1f)
        {
           
            if (Vector3.Distance(transform.position, player.position) > approachRange)
            {
                if (movingLeft)
                    spriteRenderer.flipX = false;
                else
                    spriteRenderer.flipX = true;
                isApproaching = false;

                currentAnimationCoroutine = StartCoroutine(MovePattern());
                yield break;
            }

            transform.position = Vector3.MoveTowards(transform.position, player.position, approachSpeed * Time.deltaTime);
            yield return null;
        }

        isApproaching = false;
        currentAnimationCoroutine = StartCoroutine(MovePattern());
    }

    private void Die()
    {
        animator.SetBool("IsDie", true);

        Destroy(gameObject, 2f);
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                TakeDamage(1);
            }
        }
    }
}
