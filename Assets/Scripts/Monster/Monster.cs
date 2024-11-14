using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
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

    private int health = 3;             // 적의 체력
    private bool isHit = false;         // 피격 상태 체크
    private string currentAnimState;     // 현재 애니메이션 상태 확인을 위한 변수
    void Start()
    {
        
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;  // 원래 스프라이트 저장
        fadeManager = FindObjectOfType<FadeManager>(); // 페이드 매니저 찾기
        animator = GetComponent<Animator>();
        fixedY = transform.position.y;
        currentAnimationCoroutine = StartCoroutine(MovePattern()); // 이동 패턴 시작
        currentAnimState = ""; // 초기 애니메이션 상태 초기화
    }

    void Update()
    {
        animator.SetBool("IsJump", true);


        if (isHit || isApproaching) return;

        // 플레이어와의 거리 체크
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 접근 범위 내에 플레이어가 있고 접근 중이 아닌 경우에만 접근 시작
        if (distanceToPlayer <= approachRange && !isApproaching)
        {
            if (currentAnimationCoroutine != null)
            {
                StopCoroutine(currentAnimationCoroutine);
                currentAnimationCoroutine = null;
            }

            StartCoroutine(ApproachPlayer());
        }

        // Y축 고정
        //transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
    }

    // 적의 피해 처리
    private void TakeDamage(int damage)
    {
        health -= damage;   // 체력 감소

        if (isHit)
        {
            health -= damage;
            Debug.Log("추가피격");
            return;
        }
        if (health <= 0)
        {
            StartCoroutine(Die());
            return;
        }
        isHit = true;
        spriteRenderer.sprite = hitSprite;
        StopAllCoroutines();
        animator.enabled = false;
        StartCoroutine(HandleHit()); // 피격 처리 시작
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
        while (true)
        {
            float targetX = movingLeft ? startPosition.x - moveDistance : startPosition.x + moveDistance;
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

            float elapsedTime = 0f;
            while (elapsedTime < 3f)
            {
                float jumpHeight = Mathf.Sin(elapsedTime * Mathf.PI) * 2f;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            movingLeft = !movingLeft;
            spriteRenderer.flipX = movingLeft;

            // 2초 대기 (Idle 애니메이션)
            animator.SetBool("IsJump", false);
            animator.SetBool("IsIdle", true);
            yield return new WaitForSeconds(2f);

            animator.SetBool("IsJump", true);
            animator.SetBool("IsIdle", false);
        }
        
    }

    IEnumerator ApproachPlayer()
    {
        isApproaching = true;
        Debug.Log("ApproachPlayer 코루틴 시작");

        Vector3 dir = (player.position - transform.position).normalized;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= 5.0f)
        {
            transform.position = new Vector3(transform.position.x + (dir.x * approachSpeed), transform.position.y, transform.position.z + (dir.z * approachSpeed));
        }
        else
        {
            isApproaching = false;
            currentAnimationCoroutine = StartCoroutine(MovePattern()); // 원래 이동 패턴 재개
        }
        yield return null;
       
  
        Debug.Log("ApproachPlayer 코루틴 종료, 이동 패턴 재개");
    }
    private IEnumerator Die()
    {
        animator.SetBool("IsDie", true);

        Debug.Log("사망 애니메이션 실행");

        yield return new WaitForSeconds(2f); 
        Destroy(gameObject); 
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
