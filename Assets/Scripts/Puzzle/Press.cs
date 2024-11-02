using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour, IPuzzle
{
    public GameObject door;
    public int maxLength = 5;

    private Animation anim;
    private bool alreadyOpen = false; // 문이 열린 적이 있는가?
    private bool alreadyOn = false; // 발판을 밟은 적이 있는가?

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public void On()
    {
        anim.Play("press_on");
        alreadyOn = true;
    }

    public void Off()
    {
        if (!alreadyOpen) return;
        anim.Play("press_off");
        Close();
        alreadyOn = false;
    }

    public void Open()
    {
        if (alreadyOpen) return;

        door.GetComponent<Animation>().Play();
        alreadyOpen = true;
    }

    public void Close()
    {

    }


    private void Update()
    {
        Debug.DrawRay(transform.position, transform.up * maxLength, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, maxLength);
        if (hit.transform.gameObject.CompareTag("Player") && !alreadyOn)
        {
            On();
            Open();
        }
        else if(alreadyOpen)
        {
            Off();
        }
    }
}
