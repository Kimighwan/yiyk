using UnityEngine;
using UnityEngine.UI;

public class BossBehavior : MonoBehaviour
{
    public float health = 1000f; // 보스 체력
    public Transform player; // 플레이어 Transform
    public float moveSpeed = 1f; // 이동 속도
    public float dashSpeed = 3f; // 대시 속도
    public GameObject toppingPrefab; // 던질 오브젝트 Prefab
    public float toppingSpeed = 2f; // 던질 오브젝트 속도
    public Slider healthBar; // 체력바

    private BossState currentState;
    private float stateTimer = 0f; // 상태 전환 타이머
    private Animator animator;
    private Vector3 dashTargetPosition; // 대시 목표 위치

    void Start()
    {
        health = 1000f; // 체력 초기화
        animator = GetComponent<Animator>();
        ChangeState(BossState.Idle); // 초기 상태는 Idle

        if (healthBar != null)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
        }
    }

    void Update()
    {
        stateTimer -= Time.deltaTime;

        if (health <= 0)
        {
            Die();
        }

        switch (currentState)
        {
            case BossState.Idle:
                HandleIdle();
                break;
            case BossState.MoveToPlayer:
                HandleMoveToPlayer();
                break;
            case BossState.DashPrepare:
                HandleDashPrepare();
                break;
            case BossState.Dashing:
                HandleDashing();
                break;
            case BossState.ThrowTopping:
                HandleThrowTopping();
                break;
        }
    }

    void ChangeState(BossState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case BossState.Idle:
                stateTimer = Random.Range(2f, 4f);
                break;
            case BossState.MoveToPlayer:
                stateTimer = Random.Range(2f, 4f);
                break;
            case BossState.DashPrepare:
                stateTimer = 2f; // 대시 준비 시간
                dashTargetPosition = player.position; // 대시 목표 위치 설정
                break;
            case BossState.Dashing:
                stateTimer = 1f; // 대시 지속 시간
                break;
            case BossState.ThrowTopping:
                stateTimer = Random.Range(2f, 4f);
                break;
        }

        animator.SetInteger("State", (int)newState); // 애니메이션 상태 전환
    }

    void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0) health = 0;

        if (healthBar != null)
        {
            healthBar.value = health;
        }
    }

    void HandleIdle()
    {
        if (stateTimer <= 0) ChangeState(RandomState());
    }

    void HandleMoveToPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (stateTimer <= 0) ChangeState(RandomState());
    }

    void HandleDashPrepare()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("IsDash"))
        {
            animator.SetTrigger("DIsDash");
        }

        if (stateTimer <= 0)
        {
            ChangeState(BossState.Dashing);
        }
    }

    void HandleDashing()
    {
        Vector3 direction = (dashTargetPosition - transform.position).normalized;
        transform.position += direction * dashSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, dashTargetPosition) < 0.1f)
        {
            ChangeState(BossState.Idle); // 목표 지점에 도달하면 Idle로 복귀
        }
    }

    void HandleThrowTopping()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("IsThrow"))
        {
            animator.SetTrigger("IsThrow");
        }

        if (stateTimer <= 0)
        {
            for (int i = 0; i < 5; i++)
            {
                ThrowTopping();
            }
            ChangeState(RandomState());
        }
    }

    void ThrowTopping()
    {
        Vector3 randomOffset = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
        Vector3 targetPosition = player.position + randomOffset;

        GameObject topping = Instantiate(toppingPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = topping.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (targetPosition - transform.position).normalized;
            rb.velocity = direction * toppingSpeed;
        }
    }

    void Die()
    {
        animator.SetTrigger("IsDie");
        Destroy(gameObject, 2f); // 사망 애니메이션 후 삭제
    }

    BossState RandomState()
    {
        return (BossState)Random.Range(0, 4); // Stunned 상태를 제외
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == BossState.Dashing)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                // 충돌로 인해 대시 중 기절 상태 전환
                stateTimer = 3f; // 기절 시간
                animator.SetTrigger("IsStunned");
                currentState = BossState.Idle; // Stunned를 별도 상태로 두지 않음
                health -= 100; // 장애물 충돌 시 추가 피해
            }
        }
    }
}

public enum BossState
{
    Idle,
    MoveToPlayer,
    DashPrepare,
    Dashing,
    ThrowTopping
}
