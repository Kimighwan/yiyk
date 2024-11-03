using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever1 : Lever
{

    public GameObject door1;
    public GameObject door2;


    private Animator doorAnim1;
    private Animator doorAnim2;

    private void Awake()
    {
        doorAnim1 = door1.GetComponent<Animator>();
        doorAnim2 = door2.GetComponent<Animator>();
    }

    public override void Open()
    {
        if (alreadyOpen) // 열려있으면 그냥 종료
            return;

        doorAnim.SetBool("isWall1Up", door.transform.position.y == 0);
        doorAnim1.SetBool("isWall2Down", door1.transform.position.y == 16);
        doorAnim2.SetBool("isWall3Up", door2.transform.position.y == 0);

        alreadyOpen = true; // 열려있다는 것을 체크
    }
}
