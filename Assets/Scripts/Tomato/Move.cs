using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float maxSpeed;
    public float knockbackForce = 10f;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private StageManager stageManager;
    private FadeManager fadeManager;
    private bool isDead = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stageManager = FindObjectOfType<StageManager>();
        fadeManager = FindObjectOfType<FadeManager>();
    }

    private void Start()
    {
        SceneLoader.Instance.Fade(Color.black, 1f, 0f, 2.0f, 0f, true);
    }

    private void Update()
    {
        if (isDead) return; // 사망 상태에서는 이동 불가

        

        // 이동 처리
        if (Input.GetButtonUp("Horizontal"))
        {
            float x = Input.GetAxis("Horizontal");
            rigid.AddForce(new Vector2(x, rigid.velocity.y));
        }

        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        if (Mathf.Abs(rigid.velocity.x) < 0.2)
            anim.SetBool("isMove", false);
        else
            anim.SetBool("isMove", true);
    }

    private void FixedUpdate()
    {
        if (isDead) return; // 사망 상태에서는 이동 불가

        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            rigid.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        anim.SetBool("isDie", true);
        rigid.velocity = Vector2.zero;
        SceneLoader.Instance.ReloadScene();
        SetPosition(transform.position);
    }

    public void SetPosition(Vector3 newPosition)
    {
        StartCoroutine(StartSetPosition(newPosition));
    }

    private IEnumerator StartSetPosition(Vector3 newPosition)
    {
        yield return new WaitForSecondsRealtime(1f);
        transform.position = newPosition;
        rigid.velocity = Vector2.zero; // 이동 초기화
        isDead = false;
    }
}