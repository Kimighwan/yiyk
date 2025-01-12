using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform respawnPoint;      // 리스폰 위치 (Prefab에서 설정)
    private bool isKilledByTrap = false; // Trap에 의한 죽음 여부

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
        {
            isKilledByTrap = true; // Trap에 의한 죽음을 기록
            RespawnGo();
        }
    }
    private IEnumerator RespawnGo()
    {
        // 1초 뒤 리스폰 처리
        yield return new WaitForSeconds(1f); // 리스폰 대기
        GameObject newMonster = Instantiate(gameObject, respawnPoint.position, Quaternion.identity); // 리스폰 위치로 새 객체 생성
        newMonster.GetComponent<Monster>().enabled = true; // 몬스터 스크립트 활성화
        isKilledByTrap = false;
    }
}
