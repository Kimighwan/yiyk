using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiscuitCollider : MonoBehaviour
{
    private Collider2D col;

    private void Awake()
    {
        col = GetComponentInParent<Collider2D>();
    }

    private void OffCollider()
    {
        col.enabled = false;
    }
}
