using UnityEngine;

public class Door : MonoBehaviour
{
    private Collider2D Collider;

    private void Awake()
    {
        Collider = GetComponent<Collider2D>();
    }
    private void OnEnable()
    {
        AudioManager.Instance.PlaySFX(SFX.Opendoor);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoader.Instance.NextStage();
            Collider.enabled = false;
        }
    }
}
