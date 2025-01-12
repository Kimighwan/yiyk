using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform respawnPoint;      // ������ ��ġ (Prefab���� ����)
    private bool isKilledByTrap = false; // Trap�� ���� ���� ����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
        {
            isKilledByTrap = true; // Trap�� ���� ������ ���
            RespawnGo();
        }
    }
    private IEnumerator RespawnGo()
    {
        // 1�� �� ������ ó��
        yield return new WaitForSeconds(1f); // ������ ���
        GameObject newMonster = Instantiate(gameObject, respawnPoint.position, Quaternion.identity); // ������ ��ġ�� �� ��ü ����
        newMonster.GetComponent<Monster>().enabled = true; // ���� ��ũ��Ʈ Ȱ��ȭ
        isKilledByTrap = false;
    }
}
