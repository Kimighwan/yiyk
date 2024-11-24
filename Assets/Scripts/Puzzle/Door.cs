using UnityEngine;

public class Door : MonoBehaviour
{
    private Collider2D Collider;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
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
            gameManager.playerInvincibility = true;
            AudioManager.Instance.PlaySFX(SFX.StageClear);
            SceneLoader.Instance.NextStage();
            Collider.enabled = false;
        }
    }
}
