using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Transform player;            // 플레이어 위치
    public Transform respawnPoint;      // 리스폰 위치 (Prefab에서 설정)
    public float moveDistance = 3f;     // 적의 이동 거리
    public float moveSpeed = 1f;        // 적의 기본 이동 속도
    public float approachSpeed = 2f;    // 플레이어 접근 시 속도
    public float approachRange = 15f;    // 접근 반응 거리
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

    private int health = 5;             // 적의 체력
    private bool isHit = false;         // 피격 상태 체크
    private bool isDie = false;         // 죽었는지 체크
    private string currentAnimState;     // 현재 애니메이션 상태 확인을 위한 변수
    private bool isKilledByTrap = false; // Trap에 의한 죽음 여부
    public GameObject monsterPrefab;

    public string monsterType;
    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;  // 원래 스프라이트 저장
        fadeManager = FindObjectOfType<FadeManager>(); // 페이드 매니저 찾기
        animator = GetComponent<Animator>();
        currentAnimationCoroutine = StartCoroutine(MovePattern()); // 이동 패턴 시작
        currentAnimState = ""; // 초기 애니메이션 상태 초기화
    }

    void Update()
    {
        if (isHit || isApproaching || isDie) return;

        // 플레이어와의 거리 체크
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (isApproaching)
        {
            UpdateFacingDirection(true);
        }
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

            if (!isDie)
                Die();
            return;
        }

        // 경직 상태가 이미 활성화된 경우에도 갱신
        if (!isHit)
        {
            isHit = true;
            animator.enabled = false;
            spriteRenderer.sprite = hitSprite;
        }

        // 코루틴 갱신
        StopAllCoroutines();
        StartCoroutine(HandleHit());
    }

    IEnumerator HandleHit()
    {
        yield return new WaitForSeconds(2f); // 피격 후 대기 시간

        // 피격 상태 복구
        spriteRenderer.sprite = originalSprite;
        isHit = false;
        animator.enabled = true;

        // 상태에 따라 행동 재개
        if (!isDie)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= approachRange)
            {
                StartCoroutine(ApproachPlayer()); // 플레이어 추격 재개
            }
            else
            {
                currentAnimationCoroutine = StartCoroutine(MovePattern()); // 이동 패턴 재개
            }
        }
    }

    // 이동 패턴
    IEnumerator MovePattern()
    {
        while (!isApproaching)
        {
            UpdateFacingDirection(false);
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

    IEnumerator ApproachPlayer()
    {
        isApproaching = true;
        animator.SetBool("IsJump", true);
        animator.SetBool("IsIdle", false);

        while (!isDie && Vector3.Distance(transform.position, player.position) > 0.1f)
        {
            if (Vector3.Distance(transform.position, player.position) > approachRange)
            {
                isApproaching = false;
                currentAnimationCoroutine = StartCoroutine(MovePattern());
                yield break;
            }

            UpdateFacingDirection(true);

            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, approachSpeed * Time.deltaTime);

            yield return null;
        }

        if (!isDie)
        {
            isApproaching = false;
            currentAnimationCoroutine = StartCoroutine(MovePattern());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
        {
            isKilledByTrap = true;
            Die();
        }
    }

    private void UpdateFacingDirection(bool usePlayerDirection = true)
    {
        if (usePlayerDirection)
        {
            // 플레이어 방향을 기준으로 방향 설정
            if (player.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true; // 플레이어가 왼쪽에 있으면 왼쪽을 바라봄
            }
            else
            {
                spriteRenderer.flipX = false; // 플레이어가 오른쪽에 있으면 오른쪽을 바라봄
            }
        }
        else
        {
            // 이동 방향(movingLeft)을 기준으로 방향 설정
            spriteRenderer.flipX = movingLeft;
        }
    }

    private void Die()
    {
        if (isDie) return;

        isDie = true;
        animator.SetBool("IsDie", true);
        StopCoroutine(ApproachPlayer());
        StopAllCoroutines();
        isApproaching = false;
        isHit = false;

        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        AudioManager.Instance.PlaySFX(SFX.EnemyDie1);
        
        if (isKilledByTrap)
        {
            StartCoroutine(Respawn(respawnPoint.position)); // 리스폰 코루틴 호출
            Destroy(gameObject, 1.2f);
        }
        else if(monsterType == "RespawnMonster")
        {
            StartCoroutine(Respawn(respawnPoint.position)); // 리스폰 코루틴 호출
            Destroy(gameObject, 1.2f);
        }
        else
        {
            Destroy(gameObject, 0.8f);
        }
       
    }
    private IEnumerator Respawn(Vector3 respawnPosition)
    {
        yield return new WaitForSeconds(1f); // 리스폰 대기 시간

        // 몬스터를 새로 생성
        if (monsterPrefab != null)
        {
            GameObject newMonster = Instantiate(monsterPrefab, respawnPosition, Quaternion.identity);
            newMonster.transform.parent = transform.parent; // 원래 부모 아래로 이동
            newMonster.GetComponent<Monster>().InitializeForRespawn(); // 리스폰 초기화
        }

        // 기존 오브젝트 삭제
        Destroy(gameObject);
        Debug.Log("기존 오브젝트 삭제 완료");
    }
    public void InitializeForRespawn()
    {
        isDie = false;
        isHit = false;
        isApproaching = false;

        // 초기 상태 복구
        animator.SetBool("IsDie", false);
        animator.Rebind();

        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }
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
