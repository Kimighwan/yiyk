using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    // �� ��ȯ �� �������� ����
    protected bool isDestroyOnLoad = false; // false�� �����ϰڴ�

    // �� Ŭ������ ����ƽ �ν��Ͻ� ����
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
        else if(instance != this)
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
        if(instance == this)
            instance = null;
    }
}
