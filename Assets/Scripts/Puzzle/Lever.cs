using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour, IPuzzle
{
    public GameObject door;

    public GameObject lever;

    private Animation doorAnim;

    private Animator leverOn;

    private bool alreadyOpen = false;
    //Animator leverOn;
    
    private void Awake()
    {
        doorAnim = door.transform.GetChild(1).GetComponent<Animation>();

        leverOn = lever.GetComponent<Animator>();
        //leverOn.StopPlayback();
    }

    public void Open() // 문 여는 함수
    {
        if (alreadyOpen) // 열려있으면 그냥 종료
            return;

        doorAnim.Play();
        Invoke("OnCol", 0.8f);

        alreadyOpen = true; // 열려있다는 것을 체크
    }

    private void OnCol()
    {
        door.GetComponent<Collider2D>().enabled = true;
    }

    public void On() // 레버 작동 함수
    {
        leverOn.SetBool("isOn", true);
    }


    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Open();
            On();
        }
    }

}
