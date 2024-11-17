using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float maxSpeed;
    public float knockbackForce = 10f;

    private Rigidbody2D rigid;
    private Vector2 moveVec;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private bool isDead = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        isDead = false;
        // SceneLoader.Instance.Fade(Color.black, 1f, 0f, 2.0f, 0f, true);
       // AudioManager.Instance.PlayBGM(BGM.IngameBGM); // 임시 BGM 재생 위치
    }

    private void Update()
    {
        if (isDead) return; // 사망 상태에서는 이동 불가


        // 이동 처리
        float x = Input.GetAxis("Horizontal");
        moveVec = new Vector2(x, rigid.velocity.y);
        rigid.AddForce(moveVec);

        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        if (moveVec == Vector2.zero/*Mathf.Abs(rigid.velocity.x) < 0.2*/)
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
            //Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            //rigid.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            StartCoroutine("DieCo");
        }
    }

    private IEnumerator DieCo()
    {
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        isDead = true;
        anim.SetBool("isDie", true);
        rigid.velocity = Vector2.zero;
        rigid.simulated = false;
        yield return new WaitForSeconds(1.0f);
        SceneLoader.Instance.ReloadScene();

        // SetPosition(transform.position);
    }

    //public void SetPosition(Vector3 newPosition)
    //{
    //    StartCoroutine(StartSetPosition(newPosition));
    //}

    //private IEnumerator StartSetPosition(Vector3 newPosition)
    //{
    //    yield return new WaitForSecondsRealtime(1f);
    //    transform.position = newPosition;
    //    rigid.velocity = Vector2.zero; // 이동 초기화
    //    isDead = false;
    //}
}