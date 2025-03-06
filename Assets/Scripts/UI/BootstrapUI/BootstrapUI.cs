using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class BootstrapUI : MonoBehaviour
{
    [Inject] private IBootstrapService bootstrapService;

    private void Awake()
    {
        LoadGameScene();
    }

    private async UniTask LoadGameScene()
    {
        await bootstrapService.BootstrapTask;

        SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
    }
}
