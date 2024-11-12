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

    public void Open() // �� ���� �Լ�
    {
        if (alreadyOpen) // ���������� �׳� ����
            return;

        doorAnim.Play();
        Invoke("OnCol", 0.8f);

        alreadyOpen = true; // �����ִٴ� ���� üũ
    }

    private void OnCol()
    {
        door.GetComponent<Collider2D>().enabled = true;
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
