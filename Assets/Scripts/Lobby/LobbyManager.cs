using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public LobbyUIController lobbyUIController { get; private set; }

    protected override void Init()
    {
        // 로비 매니저는 다른 씬으로 전환되면 삭제할 것이다.
        isDestroyOnLoad = true;

        base.Init();
    }

    private void Start()
    {
        lobbyUIController = FindObjectOfType<LobbyUIController>();
        if (!lobbyUIController)
        {
            return;
        }

        lobbyUIController.Init();
        AudioManager.Instance.PlayBGM(BGM.lobby);
    }
}
