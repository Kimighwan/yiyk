using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour, IPuzzle
{
    public GameObject door;

    public GameObject lever;

    protected Animator doorAnim;

    protected Animator leverOn;

    protected bool alreadyOpen = false;
    //Animator leverOn;
    
    private void Awake()
    {
        doorAnim = door.GetComponent<Animator>();

        leverOn = lever.GetComponent<Animator>();
        //leverOn.StopPlayback();
    }

    public virtual void Open() // �� ���� �Լ�
    {
        if (alreadyOpen) // ���������� �׳� ����
            return;

        doorAnim.SetBool("isWall1Up", door.transform.position.y == 0);

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
