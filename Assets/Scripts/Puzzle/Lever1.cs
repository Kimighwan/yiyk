using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever1 : MonoBehaviour, IPuzzle
{
    public GameObject door;
    public GameObject door1;
    public GameObject door2;

    public GameObject lever;

    private Animator doorAnim;
    private Animator doorAnim1;
    private Animator doorAnim2;

    private Animator leverOn;

    private bool alreadyOpen = false;
    //Animator leverOn;

    private void Awake()
    {
        doorAnim = door.transform.GetChild(1).GetComponent<Animator>();
        doorAnim1 = door1.transform.GetChild(1).GetComponent<Animator>();
        doorAnim2 = door2.transform.GetChild(1).GetComponent<Animator>();

        leverOn = lever.GetComponent<Animator>();
        //leverOn.StopPlayback();
    }

    public void Open() // 문 여는 함수
    {
        if (alreadyOpen) // 열려있으면 그냥 종료
            return;

        doorAnim.SetBool("isWall1Up", door.transform.localPosition.y == 0);
        doorAnim1.SetBool("isWall2Down", door1.transform.localPosition.y == 16);
        doorAnim2.SetBool("isWall3Up", door2.transform.localPosition.y == 0);

        alreadyOpen = true; // 열려있다는 것을 체크
    }

    public void On() // 레버 작동 함수
    {
        leverOn.SetBool("isOn", true);
        AudioManager.Instance.PlaySFX(SFX.LeverSwitch);
    }


    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && !alreadyOpen)
        {
            Open();
            On();
        }
    }

}
