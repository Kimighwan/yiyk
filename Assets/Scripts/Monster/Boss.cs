using UnityEngine;
using UnityEngine.UI;

public class BossBehavior : MonoBehaviour
{
    public float health = 1000f; // ���� ü��
    public Transform player; // �÷��̾� Transform
    public float moveSpeed = 1f; // �̵� �ӵ�
    public float dashSpeed = 3f; // ��� �ӵ�
    public GameObject toppingPrefab; // ���� ������Ʈ Prefab
    public float toppingSpeed = 2f; // ���� ������Ʈ �ӵ�
    public Slider healthBar; // ü�¹�

    private BossState currentState;
    private float stateTimer = 0f; // ���� ��ȯ Ÿ�̸�
    private Animator animator;
    private Vector3 dashTargetPosition; // ��� ��ǥ ��ġ

    void Start()
    {
        health = 1000f; // ü�� �ʱ�ȭ
        animator = GetComponent<Animator>();
        ChangeState(BossState.Idle); // �ʱ� ���´� Idle

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
                stateTimer = 2f; // ��� �غ� �ð�
                dashTargetPosition = player.position; // ��� ��ǥ ��ġ ����
                break;
            case BossState.Dashing:
                stateTimer = 1f; // ��� ���� �ð�
                break;
            case BossState.ThrowTopping:
                stateTimer = Random.Range(2f, 4f);
                break;
        }

        animator.SetInteger("State", (int)newState); // �ִϸ��̼� ���� ��ȯ
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
            ChangeState(BossState.Idle); // ��ǥ ������ �����ϸ� Idle�� ����
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
        Destroy(gameObject, 2f); // ��� �ִϸ��̼� �� ����
    }

    BossState RandomState()
    {
        return (BossState)Random.Range(0, 4); // Stunned ���¸� ����
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == BossState.Dashing)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                // �浹�� ���� ��� �� ���� ���� ��ȯ
                stateTimer = 3f; // ���� �ð�
                animator.SetTrigger("IsStunned");
                currentState = BossState.Idle; // Stunned�� ���� ���·� ���� ����
                health -= 100; // ��ֹ� �浹 �� �߰� ����
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
