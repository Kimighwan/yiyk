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

    public void Open() // �� ���� �Լ�
    {
        if (alreadyOpen) // ���������� �׳� ����
            return;

        doorAnim.Play();

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
