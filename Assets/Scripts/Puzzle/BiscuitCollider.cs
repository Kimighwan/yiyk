using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiscuitCollider : MonoBehaviour
{
    private Collider2D collider;

    private void Awake()
    {
        collider = GetComponentInParent<Collider2D>();
    }

    private void OffCollider()
    {
        collider.enabled = false;
    }
}
