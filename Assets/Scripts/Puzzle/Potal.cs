using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potal : MonoBehaviour
{
    public GameObject enter;
    public GameObject exit;

    public PotalController potalController;

    private WaitForSeconds teleportCoolTime = new WaitForSeconds(2.0f);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !potalController.coolTime)
        {
            potalController.coolTime = true;

            if (gameObject.name == "Enter") // ÀÔ±¸¿¡¼­ Æ÷Å» Å» ¶§
            {
                collision.transform.position = exit.transform.position;
            }
            else                           // Ãâ±¸¿¡¼­ Æ÷Å» Å» ¶§
            {
                collision.transform.position = enter.transform.position;
            }

            StartCoroutine("coolTimeCo");
        }
    }

    private IEnumerator coolTimeCo()
    {
        yield return teleportCoolTime;
        potalController.coolTime = false;
    }
}
