using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    // 씬 전환 시 삭제할지 여부
    protected bool isDestroyOnLoad = false; // false면 유지하겠다

    // 이 클래스의 스태틱 인스턴스 변수
    protected static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (instance == null)
        {
            instance = (T)this;

            if (!isDestroyOnLoad) DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        Dispose();
    }

    protected virtual void Dispose()
    {
        instance = null;
    }
}
