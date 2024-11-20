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

    private int health = 10;             // 적의 체력
    private bool isHit = false;         // 피격 상태 체크
    private string currentAnimState;     // 현재 애니메이션 상태 확인을 위한 변수

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;  // 원래 스프라이트 저장
        fadeManager = FindObjectOfType<FadeManager>(); // 페이드 매니저 찾기
        animator = GetComponent<Animator>();
        //fixedY = transform.position.y;
        currentAnimationCoroutine = StartCoroutine(MovePattern()); // 이동 패턴 시작
        currentAnimState = ""; // 초기 애니메이션 상태 초기화
    }

    void Update()
    {
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
        else
        {
            if (!isApproaching && currentAnimationCoroutine == null)
            {
                currentAnimationCoroutine = StartCoroutine(MovePattern());
            }
        }
    }

    // 적의 피해 처리
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

    // 이동 패턴
    IEnumerator MovePattern()
    {
       
        while (!isApproaching)
        {
            // 현재 방향을 기준으로 이동
            float targetX = movingLeft ? startPosition.x - 5f : startPosition.x + 5f;
           
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);


            // 이동을 시작하며 목적지로 이동
            float elapsedTime = 0f;
            while (elapsedTime < 3f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 방향을 전환
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

    // 플레이어를 향해 접근하는 코루틴
    IEnumerator ApproachPlayer()
    {
        isApproaching = true;
        animator.SetBool("IsJump", true);
        animator.SetBool("IsIdle", false);
        Debug.Log("ApproachPlayer 코루틴 시작");

        // 플레이어를 향해 회전
        Vector3 direction = (player.position - transform.position).normalized;

        // X축 방향에 따라서 flipX를 변경
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;  // 왼쪽을 바라보면 flipX를 true로 설정
            movingLeft = true;  // 왼쪽으로 이동
        }
        else
        {
            spriteRenderer.flipX = false; // 오른쪽을 바라보면 flipX를 false로 설정
            movingLeft = false; // 오른쪽으로 이동
        }

        // 플레이어 방향으로 이동
        while (Vector3.Distance(transform.position, player.position) > 0.1f)
        {
            // 플레이어와의 거리가 approachRange를 초과하면 따라가는 동작을 멈추고 원래대로 돌아감
            if (Vector3.Distance(transform.position, player.position) > approachRange)
            {
                Debug.Log("플레이어가 범위 밖으로 나갔습니다. 원래 패턴으로 돌아갑니다.");
                if (movingLeft)
                    spriteRenderer.flipX = false;
                else
                    spriteRenderer.flipX = true;
                isApproaching = false;
             
                currentAnimationCoroutine = StartCoroutine(MovePattern());  // 원래 이동 패턴으로 돌아감
                yield break;  // 코루틴 종료
            }
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);

            // X축으로만 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, approachSpeed * Time.deltaTime);

            yield return null;
        }

        Debug.Log("ApproachPlayer 코루틴 종료, 이동 패턴 재개");
       
        isApproaching = false;
        currentAnimationCoroutine = StartCoroutine(MovePattern());  // 원래 이동 패턴으로 돌아감
    }

    private void Die()
    {
        animator.SetBool("IsDie", true);

        AudioManager.Instance.PlaySFX(SFX.EnemyDie2);

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