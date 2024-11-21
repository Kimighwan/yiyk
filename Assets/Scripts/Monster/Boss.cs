using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigid2D;
    private Vector2 pos;
    private bool isCanDamaged = false;
    private int amountOfBone = 0;
    private int currentDirection = -1; // 1: 오른쪽, -1: 왼쪽

    public GameObject player;
    public GameObject throwingBone;
    public float speedx = 3f;
    public int maxScale = 5;
    private int nextPattern = 0;

    private static readonly int NONE = 0;
    private static readonly int RUSH = 1;
    private static readonly int THROW = 2;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        StartCoroutine("Rush");
    }

    void lookPlayer()
    {
        float scale = transform.localScale.z;
        transform.localScale = new Vector3(currentDirection * -1 * scale, scale, scale); //DIRECTION 변수를 이용해서 플레이어쪽을 바라보도록 한다.
    }

    IEnumerator Rush()
    {
        // 플레이어와의 충돌을 무시하도록 설정
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Collider2D bossCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(bossCollider, playerCollider, true); // 충돌 무시

        animator.SetTrigger("ready");
        lookPlayer(); // 플레이어를 바라보게 설정
        yield return new WaitForSeconds(1); // 대시 준비 시간

        animator.SetTrigger("IsDash"); // 대시 애니메이션 실행
        bool isBroken = false;

        // 벽 레이어 마스크
        int wallLayerMask = LayerMask.GetMask("Wall");
        float rayLength = 5f;

        while (!isBroken)
        {
            yield return new WaitForSeconds(0.1f);

            if (speedx > rigid2D.velocity.x * currentDirection)
            {
                rigid2D.AddForce(transform.right * currentDirection * 1000);
            }

            Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y + 0.5f);
            Debug.DrawRay(rayOrigin, transform.right * currentDirection * rayLength, Color.red, 1f);

            // 벽과만 충돌 감지
            RaycastHit2D rayHit = Physics2D.Raycast(rayOrigin, transform.right * currentDirection, rayLength, wallLayerMask);
            if (rayHit.collider != null)
            {
                // 벽에 충돌하면 대시를 멈추고 다음 패턴으로 이동
                Debug.Log($"부딪힌 벽: {rayHit.collider.name}");
                rigid2D.velocity = Vector2.zero;
                isBroken = true;
            }
        }

        // 대시가 끝난 후, 플레이어와의 충돌을 복구
        Physics2D.IgnoreCollision(bossCollider, playerCollider, false); // 충돌 복구

        animator.SetTrigger("IsStunned"); // 스턴 애니메이션 실행
        yield return StartCoroutine(stunCounter()); // 스턴 카운터 시작
        ToggleDirection(); // 방향 전환
        nextPattern = THROW; // 던지기 패턴으로 설정
        yield return new WaitForSeconds(3);

        nextPatternPlay(); // 다음 패턴 실행
    }

    IEnumerator Throw()
    {
        lookPlayer();
        animator.SetTrigger("IsThrow");
        yield return new WaitForSeconds(1.5f);

        // 공을 무작위 각도로 플레이어를 향해 던지기
        for (int i = 0; i < 5; i++) //공의 수만큼 공을 소환하고 각각 무작위 각도로 쏘아줌
        {
            GameObject go = Instantiate(throwingBone, new Vector3(transform.position.x + 3 * currentDirection, transform.position.y, transform.position.z), Quaternion.Euler(0f, 20f, 0f)) as GameObject;
            Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
            go.GetComponent<Rigidbody2D>().velocity = directionToPlayer * 15f; // 플레이어 방향으로 던지기
        }

        amountOfBone = 0;

        if (0 == Random.Range(0, 2)) nextPattern = RUSH; //다음 패턴을 랜덤으로 정해준다.

        yield return new WaitForSeconds(3);

        nextPatternPlay();
    }

    IEnumerator stunCounter() //위에서 호출되는 함수로 4초가 지난 후에 stunned를 false로 설정한다.
    {
        yield return new WaitForSeconds(4);
        animator.SetTrigger("IsIdle");
    }

    void ToggleDirection()
    {
        currentDirection *= -1; // 방향 전환
    }

    void nextPatternPlay()
    {
        switch (nextPattern)
        {
            case 1:
                StartCoroutine(Rush());
                break;
            case 2:
                StartCoroutine(Throw());
                break;
        }
    }
}
