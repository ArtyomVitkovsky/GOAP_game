using System;
using Cysharp.Threading.Tasks;
using Services;
using Services.TickableService;
using UnityEngine;
using Zenject;

public abstract class Follower : MonoBehaviour
{
    [Inject] protected IBootstrapService bootstrapService;
    [Inject] protected ITickableService tickableService;
    
    [SerializeField] protected Transform target;
    [SerializeField] protected Vector3 offset;

    private void Awake()
    {
        Initialize();
    }

    protected virtual async UniTask Initialize()
    {
        await bootstrapService.BootstrapTask;
        
        AddToTickable();
    }

    protected virtual void AddToTickable()
    {
        tickableService.AddUpdateTickable(new TickableEntity(Follow));
    }

    protected virtual void Follow()
    {
        transform.position = target.position + offset;
    }
}