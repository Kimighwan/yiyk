using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour, IPuzzle
{
    public GameObject door;
    public int maxLength = 1;

    private Animator anim;
    private bool alreadyOpen = false; // ���� ���� ���� �ִ°�?

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void On()
    {
        anim.SetTrigger("isPress");
    }

    //public void Off()
    //{
    //    if (!alreadyOpen) return;
    //    anim.Play("press_off");
    //    Close();
    //    alreadyOn = false;
    //}

    public void Open()
    {
        if (alreadyOpen) return;

        door.GetComponent<Animation>().Play();

        alreadyOpen = true;
    }

    //public void Close()
    //{

    //}


    private void Update()
    {
        Debug.DrawRay(transform.position, transform.up * maxLength, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, maxLength, LayerMask.GetMask("Player"));
        if(hit.collider != null)
        {
            if (hit.transform.gameObject.CompareTag("Player") && !alreadyOpen)
            {
                On();
                Open();
            }
        }
        //else if(alreadyOpen)
        //{
        //    Off();
        //}
    }
}
