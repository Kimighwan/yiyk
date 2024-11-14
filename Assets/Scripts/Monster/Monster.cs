using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Transform player;            // �÷��̾� ��ġ
    public float moveDistance = 3f;     // ���� �̵� �Ÿ�
    public float moveSpeed = 1f;        // ���� �⺻ �̵� �ӵ�
    public float approachSpeed = 2f;    // �÷��̾� ���� �� �ӵ�
    public float approachRange = 5f;    // ���� ���� �Ÿ�
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

    private int health = 3;             // ���� ü��
    private bool isHit = false;         // �ǰ� ���� üũ
    private string currentAnimState;     // ���� �ִϸ��̼� ���� Ȯ���� ���� ����
    void Start()
    {
        
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;  // ���� ��������Ʈ ����
        fadeManager = FindObjectOfType<FadeManager>(); // ���̵� �Ŵ��� ã��
        animator = GetComponent<Animator>();
        fixedY = transform.position.y;
        currentAnimationCoroutine = StartCoroutine(MovePattern()); // �̵� ���� ����
        currentAnimState = ""; // �ʱ� �ִϸ��̼� ���� �ʱ�ȭ
    }

    void Update()
    {
        animator.SetBool("IsJump", true);


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

        // Y�� ����
        //transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
    }

    // ���� ���� ó��
    private void TakeDamage(int damage)
    {
        health -= damage;   // ü�� ����

        if (isHit)
        {
            health -= damage;
            Debug.Log("�߰��ǰ�");
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
        StartCoroutine(HandleHit()); // �ǰ� ó�� ����
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

            // 2�� ��� (Idle �ִϸ��̼�)
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
        Debug.Log("ApproachPlayer �ڷ�ƾ ����");

        Vector3 dir = (player.position - transform.position).normalized;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= 5.0f)
        {
            transform.position = new Vector3(transform.position.x + (dir.x * approachSpeed), transform.position.y, transform.position.z + (dir.z * approachSpeed));
        }
        else
        {
            isApproaching = false;
            currentAnimationCoroutine = StartCoroutine(MovePattern()); // ���� �̵� ���� �簳
        }
        yield return null;
       
  
        Debug.Log("ApproachPlayer �ڷ�ƾ ����, �̵� ���� �簳");
    }
    private IEnumerator Die()
    {
        animator.SetBool("IsDie", true);

        Debug.Log("��� �ִϸ��̼� ����");

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
