using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour, IPuzzle
{
    //public GameObject door;
    public GameObject door1;
    //public GameObject door2;

    public GameObject lever;

    //private Animator doorAnim;
    private Animator doorAnim1;
    //private Animator doorAnim2;

    private Animator leverOn;

    private bool alreadyOpen = false;
    //Animator leverOn;
    
    private void Awake()
    {
        //doorAnim = door.GetComponent<Animator>();
        doorAnim1 = door1.GetComponent<Animator>();
        //doorAnim2 = door2.GetComponent <Animator>();

        leverOn = lever.GetComponent<Animator>();
        //leverOn.StopPlayback();
    }

    public void Open() // 문 여는 함수
    {
        if (alreadyOpen) // 열려있으면 그냥 종료
            return;

        //doorAnim.Play();
        doorAnim1.SetBool("isWall2Down", door1.transform.position.y == 16);
        //doorAnim2.Play();

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
            Debug.Log("Lever clicked");
            Open();
            On();
        }
    }

}
