using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        doorAnim = door.GetComponent<Animator>();
        doorAnim1 = door1.GetComponent<Animator>();
        doorAnim2 = door2.GetComponent<Animator>();

        leverOn = lever.GetComponent<Animator>();
    }

    public void Open()
    {
        if (alreadyOpen) // ���������� �׳� ����
            return;

        doorAnim.SetBool("isWall1Up", door.transform.localPosition.y == 0);
        doorAnim1.SetBool("isWall2Down", door1.transform.localPosition.y == 16);
        doorAnim2.SetBool("isWall3Up", door2.transform.localPosition.y == 0);

        alreadyOpen = true; // �����ִٴ� ���� üũ
    }

    public void On() // ���� �۵� �Լ�
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
