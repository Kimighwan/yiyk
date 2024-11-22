using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeUp : MonoBehaviour
{
    public float speed = 0.1f;


    void Update()
    {
        // transform.position = Vector3.MoveTowards(startPos.position, endPos.position, speed * Time.deltaTime);
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
