using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public GameObject[] stages;
    public Transform[] stageStartPositions;
    public int currentStageIndex = 0;

    private void Start()
    {
        ActivateStage(currentStageIndex);
    }

    public void ActivateStage(int stageIndex)
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(i == stageIndex);
        }

        Move player = FindObjectOfType<Move>();
        if (player != null && stageIndex < stageStartPositions.Length)
        {
            player.SetPosition(stageStartPositions[stageIndex].position);
        }

        currentStageIndex = stageIndex;
    }

    public void NextStage()
    {
        if (currentStageIndex < stages.Length - 1)
        {
            currentStageIndex++;
            ActivateStage(currentStageIndex);
        }
        else
        {
            Debug.Log("모든 스테이지를 완료했습니다!");
        }
    }
}