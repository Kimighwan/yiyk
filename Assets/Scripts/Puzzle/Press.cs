using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour, IPuzzle
{
    public GameObject door;
    public int maxLength = 5;

    private Animation anim;
    private bool alreadyOpen = false; // 문이 열린 적이 있는가?

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public void On()
    {
        anim.Play();
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
