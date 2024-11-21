using System.Collections;
using UnityEngine;

public class Pasta : MonoBehaviour
{
    public Transform player;
    public float moveDistance = 3f;
    public float moveSpeed = 1f;
    public float approachSpeed = 2f;
    public float approachRange = 5f;
    private Vector3 startPosition;
    private bool movingLeft = true;
    private bool isApproaching = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public Sprite hitSprite;
    private Sprite originalSprite;
    private Coroutine currentAnimationCoroutine;
    private int health = 10;
    private bool isHit = false;
    private bool isDie = false;

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        animator = GetComponent<Animator>();
        currentAnimationCoroutine = StartCoroutine(MovePattern());
    }

    void Update()
    {
        // �ǰ� ���� �Ǵ� ���� ���¶�� �ൿ ����
        if (isHit || isDie) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �÷��̾� �߰� ����
        if (distanceToPlayer <= approachRange && !isApproaching)
        {
            StopCurrentAction(); // ���� ���� ����
            StartCoroutine(ApproachPlayer());
        }
        else if (!isApproaching && currentAnimationCoroutine == null)
        {
            currentAnimationCoroutine = StartCoroutine(MovePattern());
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (!isDie)
                Die();
            return;
        }

        if (isHit) return; // �̹� �ǰ� ���¶�� �߰� ó������ ����

        isHit = true;
        spriteRenderer.sprite = hitSprite;

        StopCurrentAction(); // ���� ��� �ൿ ����

        animator.enabled = false;

        // �ǰ� �� 1�� ����
        StartCoroutine(HandleHit());
    }

    IEnumerator HandleHit()
    {
        yield return new WaitForSeconds(1f); // 1�� ���� ����

        // �ǰ� �� ���� ����
        spriteRenderer.sprite = originalSprite;
        isHit = false;
        animator.enabled = true;

        // �ǰ� �� �̵� ���� �簳
        if (!isDie)
        {
            if (isApproaching)
            {
                StartCoroutine(ApproachPlayer());
            }
            else
            {
                currentAnimationCoroutine = StartCoroutine(MovePattern());
            }
        }
    }

    IEnumerator MovePattern()
    {
        while (!isApproaching && !isDie)
        {
            float targetX = movingLeft ? startPosition.x - 5f : startPosition.x + 5f;
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

            float elapsedTime = 0f;
            while (elapsedTime < 3f)
            {
                if (isHit) yield break; // �ǰ� ���¶�� �ߴ�

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
            if (isHit) yield break; // �ǰ� ���¶�� �ߴ�

            if (Vector3.Distance(transform.position, player.position) > approachRange)
            {
                isApproaching = false;
                if (direction.x < 0)
                {
                    spriteRenderer.flipX = false;
                    movingLeft = true;
                }
                else
                {
                    spriteRenderer.flipX = true;
                    movingLeft = false;
                }
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
        isDie = true;
        animator.SetBool("IsDie", true);

        AudioManager.Instance.PlaySFX(SFX.EnemyDie2);
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) { Destroy(collider); }
        Destroy(gameObject, 2f);
    }

    private void StopCurrentAction()
    {
        // ���� ���� ���� ��� �ڷ�ƾ ����
        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
            currentAnimationCoroutine = null;
        }

        isApproaching = false; // �߰� ���� �ʱ�ȭ
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
