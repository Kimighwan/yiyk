using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    private StageManager stageManager;
    private FadeManager fadeManager;

    private void Awake()
    {
        stageManager = FindObjectOfType<StageManager>();
        fadeManager = FindObjectOfType<FadeManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            SceneLoader.Instance.ReloadScene();
        }
    }
}
