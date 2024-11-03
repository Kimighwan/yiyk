using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Transform player;            // �÷��̾� ��ġ
    public float moveDistance = 3f;     // ���� �̵� �Ÿ�
    public float moveSpeed = 1f;        // ���� �⺻ �̵� �ӵ�
    public float approachSpeed = 2f;    // �÷��̾� ���� �� �ӵ�
    public float approachRange = 5f;    // ���� ���� �Ÿ�
    public float approachDuration = 1f; // ���� ���� �ð�
    private float fixedY;
    private Animator animator;

    private Vector3 startPosition;      // ���� ��ġ
    private bool movingLeft = true;     // �̵� ���� üũ
    private bool isApproaching = false; // �÷��̾� ���� �� ����
    private SpriteRenderer spriteRenderer; // ��������Ʈ ������
    private FadeManager fadeManager;    // ���̵� �Ŵ���
    public Sprite hitSprite;
    private Coroutine currentAnimationCoroutine;

    private int health = 3;             // ���� ü��
    private bool isHit = false;          // �ǰ� ���� üũ

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        fadeManager = FindObjectOfType<FadeManager>(); // ���̵� �Ŵ��� ã��
        animator = GetComponent<Animator>();
        fixedY = transform.position.y;
        currentAnimationCoroutine = StartCoroutine(MovePattern()); // �̵� ���� ����
    }

    void Update()
    {
        if (isHit || isApproaching) return;

        // �÷��̾���� �Ÿ� üũ
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // ���� ���� ���� �÷��̾ �ְ� ���� ���� �ƴ� ��쿡�� ���� ����
        if (distanceToPlayer <= approachRange && !isApproaching)
        {
            StopCoroutine(currentAnimationCoroutine);
            StartCoroutine(ApproachPlayer());
        }

        // ���콺 ���� Ŭ�� ����
       

        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
    }

    // �÷��̾�� �浹 ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fadeManager.FadeOutAndRestart(); // ���̵� �ƿ� �� �� �����
        }
    }

    // ���� ���� ó��
    private void TakeDamage(int damage)
    {
        spriteRenderer.sprite = hitSprite;
        Debug.Log("�ǰ� �̹��� ����");
        health -= damage; // ü�� ����
        isHit = true; // �ǰ� ���� ����
        animator.enabled = false;
        StartCoroutine(HandleHit()); // �ǰ� ó�� ����

        if (health <= 0)
        {
            Destroy(gameObject); // ü���� 0 ������ ��� ������Ʈ �ı�
        }
    }

    IEnumerator HandleHit()
    {
        yield return new WaitForSeconds(2f);
        spriteRenderer.sprite = null;
        isHit = false; // �ǰ� ���� ����
        animator.enabled = true;
        currentAnimationCoroutine = StartCoroutine(MovePattern()); // �̵� ���� �簳
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

            // ���� ��ȯ �� 2�� ��� �� Idle �ִϸ��̼�
            animator.Play("Enemy_Idle");
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator ApproachPlayer()
    {
        isApproaching = true;
        float elapsedTime = 0f;

        float fixedY = transform.position.y;

        // �÷��̾ ���󰡴� ���� Jump �ִϸ��̼� ����
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
            if (hit.collider != null && hit.collider.gameObject == gameObject) // Ŭ���� ��ü�� ���� Monster���� Ȯ��
            {
                TakeDamage(1);
                Debug.Log("���� Ŭ��");
            }
        }
    }
}
