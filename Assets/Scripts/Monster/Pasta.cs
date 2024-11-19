using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pasta : MonoBehaviour
{
    public Transform player;            // 플레이어 위치
    public float moveDistance = 3f;     // 적의 이동 거리
    public float moveSpeed = 1f;        // 적의 기본 이동 속도
    public float approachSpeed = 2f;    // 플레이어 접근 시 속도
    public float approachRange = 5f;    // 접근 반응 거리
    private float fixedY;
    private Animator animator;

    private Vector3 startPosition;      // 시작 위치
    private bool movingLeft = true;     // 이동 방향 체크
    private bool isApproaching = false; // 플레이어 접근 중 여부
    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러
    private FadeManager fadeManager;    // 페이드 매니저
    public Sprite hitSprite;
    private Sprite originalSprite;      // 원래 스프라이트 저장
    private Coroutine currentAnimationCoroutine;

    private int health = 10;             // 적의 체력
    private bool isHit = false;         // 피격 상태 체크
    private string currentAnimState;     // 현재 애니메이션 상태 확인을 위한 변수

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
