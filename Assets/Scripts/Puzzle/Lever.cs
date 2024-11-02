using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour, IPuzzle
{
    public GameObject door;
    public GameObject lever;
    private SpriteRenderer spriteRenderer;
    private Animation doorAnim;
    private Animation leverOn;
    private bool alreadyOpen = false;
    //Animator leverOn;
    
    private void Awake()
    {
        doorAnim = door.GetComponent<Animation>();
        leverOn = lever.GetComponent<Animation>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //leverOn.StopPlayback();
    }

    public void Open()
    {
        if (alreadyOpen)
            return;
        leverOn.Play("Lever");
        doorAnim.Play("door_open");
        alreadyOpen = true;
    }

    public void On()
    {
        //spriteRenderer.flipX = true;
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
