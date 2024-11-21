using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float maxSpeed = 10.0f;
    public float knockbackForce = 10f;

    private Rigidbody2D rigid;
    private Vector3 moveVec;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private bool isDead = false;
    private float speedCheck;

    [Header("Move")]
    private float h; // 수평 방향

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        isDead = false;
    }

    private void Update()
    {
        // Stop Speed
        // if (Input.GetButtonUp("Horizontal"))

        // Sprite
        if (Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // Anim
        anim.SetBool("isMove", Mathf.Abs(h) > 0.2f);
    }

    private void FixedUpdate()
    {
        // Move
        h = Input.GetAxis("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // Max Speed
        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Trap"))
        {
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
}