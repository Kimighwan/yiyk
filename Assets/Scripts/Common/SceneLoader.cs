using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    StartScene,
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage5,
    Stage6,
    Stage7,
    Stage8,
    Stage9,
    Stage10,
    Clear,
}

public class SceneLoader : SingletonBehaviour<SceneLoader>
{
    private int curStageNumber = 2; // 2 : Stage1 �� �ε���
    public void LoadScene(SceneType sceneType)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneType.ToString());
    }

    public void ReloadScene()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public AsyncOperation LoadSceneAsync(SceneType sceneType)
    {
        Time.timeScale = 1f;
        return SceneManager.LoadSceneAsync(sceneType.ToString());
    }

    public void NextStage()
    {
        LoadScene((SceneType)(SceneManager.GetActiveScene().buildIndex + 1)); // ���� ���� �ε��� + 1�� ���� ������ �̵�
    }
}
