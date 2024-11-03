using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearMove : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneLoader.Instance.LoadScene(SceneType.Clear);
    }
}
