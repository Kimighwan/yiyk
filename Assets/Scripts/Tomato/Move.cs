using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour
{
    public float maxSpeed;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private bool isDead = false; // ��� ���� üũ
    private bool restartRequested = false; // ����� ��û üũ
    private float rKeyHoldTime = 0f; // R Ű ���� �ð�

    private FadeManager fadeManager;
    private StageManager stageManager;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        fadeManager = FindObjectOfType<FadeManager>();
        stageManager = FindObjectOfType<StageManager>();
    }

    private void Update()
    {
        if (isDead)
        {
            // R Ű�� ������ ������ �ð� ����
            if (Input.GetKey(KeyCode.R))
            {
                rKeyHoldTime += Time.unscaledDeltaTime;

                if (rKeyHoldTime >= 3f)
                {
                    restartRequested = false;
                    RestartStage();
                }
            }

            // 3�� ���� ������� ���� ������ ���̵� �ƿ�
            if (restartRequested && rKeyHoldTime < 3f)
            {
                StartCoroutine(FadeToMainMenu());
            }
            return; // ��� ���¿����� �̵� �Ұ�
        }
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
        if (isDead) return;

        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
    }
    public void Die()
    {
        isDead = true;
        restartRequested= true;
        anim.SetBool("isDie", true);
        anim.SetBool("isMove", false);
      
        Invoke("DealthAnimation",2f);
    }
    
    private void DealthAnimation()
    {
        Time.timeScale = 0;
    }

    // ���̵� �ƿ� �� ���� ȭ������ �̵�
    IEnumerator FadeToMainMenu()
    {
        yield return new WaitForSeconds(3f);
        fadeManager.FadeOutAndRestart();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("StartScene"); 
    }

    private void RestartStage()
    {
        isDead = false;
        Time.timeScale = 1;
        anim.SetBool("isDie", false);
        anim.SetBool("isMove", true);
        restartRequested = false;
        stageManager.ActivateStage(stageManager.currentStageIndex);
    }
}
