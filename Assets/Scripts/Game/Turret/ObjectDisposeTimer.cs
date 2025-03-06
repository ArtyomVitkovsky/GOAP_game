using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Turret
{
    public class ObjectDisposeTimer : MonoBehaviour
    {
        [SerializeField] private float time = 10;

        private CancellationTokenSource cts;

        private void Awake()
        {
            DelayedDispose();
        }

        private async UniTask DelayedDispose()
        {
            cts = new CancellationTokenSource();
            
            await UniTask.Delay((int)(time * 1000))
                .AttachExternalCancellation(cts.Token)
                .SuppressCancellationThrow();

            if (cts.IsCancellationRequested) return;
            
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            cts?.Cancel();
        }
    }
}