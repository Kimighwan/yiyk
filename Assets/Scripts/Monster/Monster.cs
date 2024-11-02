using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Transform player;            // 플레이어 위치
    public float moveDistance = 3f;     // 적의 이동 거리
    public float moveSpeed = 1f;        // 적의 기본 이동 속도
    public float approachSpeed = 2f;    // 플레이어 접근 시 속도
    public float approachRange = 5f;    // 접근 반응 거리
    public float approachDuration = 1f;  // 접근 지속 시간
    private float fixedY;
    private Animator animator;

    private Vector3 startPosition;      // 시작 위치
    private bool movingLeft = true;     // 이동 방향 체크
    private bool isApproaching = false; // 플레이어 접근 중 여부
    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러
    private FadeManager fadeManager;    // 페이드 매니저
    public Sprite hitSprite;
    private Coroutine currentAnimationCoroutine;

    private int health = 3;             // 적의 체력
    private bool isHit = false;          // 피격 상태 체크

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        fadeManager = FindObjectOfType<FadeManager>(); // 페이드 매니저 찾기
        animator = GetComponent<Animator>();
        fixedY = transform.position.y;
        currentAnimationCoroutine = StartCoroutine(MovePattern()); // 이동 패턴 시작
    }

    void Update()
    {
        if (isHit) return;

        // 플레이어와의 거리 체크
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= approachRange && !isApproaching)
        {
            StopAllCoroutines();
            StartCoroutine(ApproachPlayer());
        }

        // 마우스 왼쪽 클릭 감지
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

    // 플레이어와 충돌 감지
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fadeManager.FadeOutAndRestart(); // 페이드 아웃 및 씬 재시작
        }
    }

    // 적의 피해 처리
    private void TakeDamage(int damage)
    {
        spriteRenderer.sprite = hitSprite;
        Debug.Log("피격 이미지 생성");
        health -= damage; // 체력 감소
        isHit = true; // 피격 상태 설정
        animator.enabled = false; // 애니메이션 중지
        StartCoroutine(HandleHit()); // 피격 처리 시작

        if (health <= 0)
        {
            Destroy(gameObject); // 체력이 0 이하일 경우 오브젝트 파괴
        }
    }

    IEnumerator HandleHit()
    {
        yield return new WaitForSeconds(2f); // 2초 동안 피격 이미지 표시
        spriteRenderer.sprite = null; // 피격 이미지 제거
        isHit = false; // 피격 상태 해제
        animator.enabled = true; // 애니메이션 재개
        currentAnimationCoroutine = StartCoroutine(MovePattern()); // 이동 패턴 재개
    }

    IEnumerator MovePattern()
    {
        while (true)
        {
            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(1f);

            // 이동할 위치 설정
            float targetX = movingLeft ? startPosition.x - moveDistance : startPosition.x + moveDistance;
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

            // 목표 위치까지 이동
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                animator.SetTrigger("Jump");
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // 이동 방향 전환
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
        StartCoroutine(MovePattern()); // 이동 패턴 재개
    }
}