using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour, IPuzzle
{
    public GameObject door;
    public int maxLength = 1;

    private Animation dorreAnim;
    private Animator animator;
    private bool alreadyOpen = false; // 문이 열린 적이 있는가?

    private void Awake()
    {
        dorreAnim = door.transform.GetChild(1).GetComponent<Animation>();
        animator = GetComponent<Animator>();
    }

    public void On()
    {
        animator.SetTrigger("isPressed");
        AudioManager.Instance.PlaySFX(SFX.JellyButton);
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

        dorreAnim.Play();
        door.transform.GetChild(2).gameObject.SetActive(true);
        //Invoke("OnCol", 0.8f);

        alreadyOpen = true;
    }

    private void OnCol()
    {
        door.GetComponent<Collider2D>().enabled = true;
    }


    private void Update()
    {
        Debug.DrawRay(transform.position, transform.up * maxLength, Color.red);

        RaycastHit2D hitTomato = Physics2D.Raycast(transform.position, transform.up, maxLength, LayerMask.GetMask("Player"));
        RaycastHit2D hitEnemy = Physics2D.Raycast(transform.position, transform.up, maxLength, LayerMask.GetMask("Enemy"));
        if (hitTomato.collider != null || hitEnemy.collider != null)
        {
            if (!alreadyOpen)
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
