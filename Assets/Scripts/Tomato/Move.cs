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
    private bool isDead;
    private float speedCheck;

    private GameManager gameManager;

    [Header("Move")]
    private float h; // Horizontal Direction

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        isDead = false;
    }

    private void Update()
    {
        if (gameManager.playerInvincibility || isDead) return;
        
        // Stop Speed
        if (Input.GetButtonUp("Horizontal"))
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        // Sprite
        if (Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // Anim
        anim.SetBool("isMove", Mathf.Abs(h) > 0.2f);
    }

    private void FixedUpdate()
    {
        if (gameManager.playerInvincibility || isDead)
        {
            rigid.velocity = Vector2.zero;
            return;
        }

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
            if (gameManager.playerInvincibility) return;

            StartCoroutine("DieCo");
        }
    }

    private IEnumerator DieCo()
    {
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        AudioManager.Instance.PlaySFX(SFX.PlayerDie);
        isDead = true;
        anim.SetBool("isDie", true);
        rigid.velocity = Vector2.zero;
        rigid.simulated = false;
        yield return new WaitForSeconds(1.0f);
        SceneLoader.Instance.ReloadScene();

        // SetPosition(transform.position);
    }
}