using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public GameObject[] stages;
    public int currentStageIndex = 0; 

    private void Start()
    {
        ActivateStage(currentStageIndex); 
    }

    public void ActivateStage(int stageIndex)
    {
        foreach (var stage in stages)
        {
            stage.SetActive(false);
        }

        stages[stageIndex].SetActive(true);
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
