using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnEnable()
    {
        AudioManager.Instance.PlaySFX(SFX.Opendoor);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoader.Instance.NextStage();
        }
    }
}
