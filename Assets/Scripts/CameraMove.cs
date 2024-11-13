using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform tomato;
    [SerializeField] float smoothing = 0.2f;
    [SerializeField] Vector2 minCameraBoundary;
    [SerializeField] Vector2 maxCameraBoundary;


    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(tomato.position.x, tomato.position.y, transform.position.z);

        //targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.x, maxCameraBoundary.x);
        //targetPos.y = Mathf.Clamp(targetPos.y, minCameraBoundary.y, maxCameraBoundary.y);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }

    //private void LateUpdate()
    //{
    //    Vector3 targetPos = new Vector3(tomato.position.x, tomato.position.y, transform.position.z);
    //    transform.position = targetPos;
    //}
}
