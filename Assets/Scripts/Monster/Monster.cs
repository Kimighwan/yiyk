using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Transform player;            // 플레이어 위치
    public float moveDistance = 3f;     // 적의 이동 거리
    public float moveSpeed = 1f;        // 적의 기본 이동 속도
    public float approachSpeed = 2f;    // 플레이어 접근 시 속도
    public float approachRange = 5f;    // 접근 반응 거리
    public float approachDuration = 1f; // 접근 지속 시간
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
        if (isHit || isApproaching) return;

        // 플레이어와의 거리 체크
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // 접근 범위 내에 플레이어가 있고 접근 중이 아닌 경우에만 접근 시작
        if (distanceToPlayer <= approachRange && !isApproaching)
        {
            StopCoroutine(currentAnimationCoroutine);
            StartCoroutine(ApproachPlayer());
        }

        // 마우스 왼쪽 클릭 감지
       

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
        animator.enabled = false;
        StartCoroutine(HandleHit()); // 피격 처리 시작

        if (health <= 0)
        {
            Destroy(gameObject); // 체력이 0 이하일 경우 오브젝트 파괴
        }
    }

    IEnumerator HandleHit()
    {
        yield return new WaitForSeconds(2f);
        spriteRenderer.sprite = null;
        isHit = false; // 피격 상태 해제
        animator.enabled = true;
        currentAnimationCoroutine = StartCoroutine(MovePattern()); // 이동 패턴 재개
    }

    IEnumerator MovePattern()
    {
        while (true)
        {
            animator.Play("Enemy_Idle");
            float targetX = movingLeft ? startPosition.x - moveDistance : startPosition.x + moveDistance;
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            movingLeft = !movingLeft;
            spriteRenderer.flipX = movingLeft;

            // 방향 전환 시 2초 대기 및 Idle 애니메이션
            animator.Play("Enemy_Idle");
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator ApproachPlayer()
    {
        isApproaching = true;
        float elapsedTime = 0f;

        float fixedY = transform.position.y;

        // 플레이어를 따라가는 동안 Jump 애니메이션 실행
        animator.Play("Enemy_Jump");

        while (elapsedTime < approachDuration)
        {
            Vector3 targetPosition = new Vector3(player.position.x, fixedY, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, approachSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isApproaching = false;
        currentAnimationCoroutine = StartCoroutine(MovePattern());
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && !isHit)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject) // 클릭한 객체가 현재 Monster인지 확인
            {
                TakeDamage(1);
                Debug.Log("몬스터 클릭");
            }
        }
    }
}
