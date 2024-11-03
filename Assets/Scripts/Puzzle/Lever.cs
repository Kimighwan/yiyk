using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour, IPuzzle
{
    public GameObject door;

    public GameObject lever;

    protected Animation doorAnim;

    protected Animator leverOn;

    protected bool alreadyOpen = false;
    //Animator leverOn;
    
    private void Awake()
    {
        doorAnim = door.GetComponent<Animation>();

        leverOn = lever.GetComponent<Animator>();
        //leverOn.StopPlayback();
    }

    public void Open() // 문 여는 함수
    {
        if (alreadyOpen) // 열려있으면 그냥 종료
            return;

        doorAnim.Play();

        alreadyOpen = true; // 열려있다는 것을 체크
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
