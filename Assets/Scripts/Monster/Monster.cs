using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Transform player;            // �÷��̾� ��ġ
    public float moveDistance = 3f;     // ���� �̵� �Ÿ�
    public float moveSpeed = 1f;        // ���� �⺻ �̵� �ӵ�
    public float approachSpeed = 2f;    // �÷��̾� ���� �� �ӵ�
    public float approachRange = 15f;    // ���� ���� �Ÿ�
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
    private bool isDie = false;         // �׾����� üũ
    private string currentAnimState;     // ���� �ִϸ��̼� ���� Ȯ���� ���� ����

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;  // ���� ��������Ʈ ����
        fadeManager = FindObjectOfType<FadeManager>(); // ���̵� �Ŵ��� ã��
        animator = GetComponent<Animator>();
        //fixedY = transform.position.y;
        currentAnimationCoroutine = StartCoroutine(MovePattern()); // �̵� ���� ����
        currentAnimState = ""; // �ʱ� �ִϸ��̼� ���� �ʱ�ȭ
    }

    void Update()
    {
        if (isHit || isApproaching) return;

        // �÷��̾���� �Ÿ� üũ
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // ���� ���� ���� �÷��̾ �ְ� ���� ���� �ƴ� ��쿡�� ���� ����
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

    // ���� ���� ó��
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
        if (isHit) return;

        isHit = true;
        spriteRenderer.sprite = hitSprite;
        StopAllCoroutines();
        animator.enabled = false;
        StartCoroutine(HandleHit());
    }



    IEnumerator HandleHit()
    {
        yield return new WaitForSeconds(2f); // �ǰ� �� ��� �ð�

        // �ǰ� ���� ����
        spriteRenderer.sprite = originalSprite;
        isHit = false;
        animator.enabled = true;

        // ���¿� ���� �ൿ �簳
        if (!isDie)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= approachRange)
            {
                StartCoroutine(ApproachPlayer()); // �÷��̾� �߰� �簳
            }
            else
            {
                currentAnimationCoroutine = StartCoroutine(MovePattern()); // �̵� ���� �簳
            }
        }
    }


    // �̵� ����
    IEnumerator MovePattern()
    {
       
        while (!isApproaching)
        {
            // ���� ������ �������� �̵�
            float targetX = movingLeft ? startPosition.x - 5f : startPosition.x + 5f;
           
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);


            // �̵��� �����ϸ� �������� �̵�
            float elapsedTime = 0f;
            while (elapsedTime < 3f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // ������ ��ȯ
            movingLeft = !movingLeft;
            spriteRenderer.flipX = movingLeft;

            // 2�� ��� (Idle �ִϸ��̼�)
            animator.SetBool("IsJump", false);
            animator.SetBool("IsIdle", true);
            yield return new WaitForSeconds(2f);

            animator.SetBool("IsJump", true);
            animator.SetBool("IsIdle", false);
        }
    }

    // �÷��̾ ���� �����ϴ� �ڷ�ƾ
    IEnumerator ApproachPlayer()
    {
        isApproaching = true;
        animator.SetBool("IsJump", true);
        animator.SetBool("IsIdle", false);
        Debug.Log("ApproachPlayer �ڷ�ƾ ����");

        // �÷��̾ ���� ȸ��
        Vector3 direction = (player.position - transform.position).normalized;

        // X�� ���⿡ ���� flipX�� ����
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;  // ������ �ٶ󺸸� flipX�� true�� ����
            movingLeft = true;  // �������� �̵�
        }
        else
        {
            spriteRenderer.flipX = false; // �������� �ٶ󺸸� flipX�� false�� ����
            movingLeft = false; // ���������� �̵�
        }

        // �÷��̾� �������� �̵�
        while (Vector3.Distance(transform.position, player.position) > 0.1f)
        {
            // �÷��̾���� �Ÿ��� approachRange�� �ʰ��ϸ� ���󰡴� ������ ���߰� ������� ���ư�
            if (Vector3.Distance(transform.position, player.position) > approachRange)
            {
                Debug.Log("�÷��̾ ���� ������ �������ϴ�. ���� �������� ���ư��ϴ�.");
                if (movingLeft)
                    spriteRenderer.flipX = false;
                else
                    spriteRenderer.flipX = true;
                isApproaching = false;
             
                currentAnimationCoroutine = StartCoroutine(MovePattern());  // ���� �̵� �������� ���ư�
                yield break;  // �ڷ�ƾ ����
            }
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);

            // X�����θ� �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, approachSpeed * Time.deltaTime);

            yield return null;
        }

        Debug.Log("ApproachPlayer �ڷ�ƾ ����, �̵� ���� �簳");
       
        isApproaching = false;
        currentAnimationCoroutine = StartCoroutine(MovePattern());  // ���� �̵� �������� ���ư�
    }

    private void Die()
    {
        if (isDie) return;

        isDie = true;
        animator.SetBool("IsDie", true);

        StopAllCoroutines();
        isApproaching = false;
        isHit = false;

        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        //StartCoroutine(ScaleUpSprite()); //��� �ִϸ��̼� ������ ����
        AudioManager.Instance.PlaySFX(SFX.EnemyDie1);

        Destroy(gameObject, 0.5f);
    }
   /* IEnumerator ScaleUpSprite()
    {
        float elapsedTime = 0f;
        float duration = 1f;
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 2f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // ���� ũ�� ����
    }*/

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