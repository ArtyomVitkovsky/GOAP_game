using Cysharp.Threading.Tasks;
using Services;
using Services.NpcLifeService;
using UnityEngine;
using Zenject;

namespace Game.NpcSystem
{
    public class NpcInterestPoint : MonoBehaviour, IInterestPoint
    {
        [Inject] private IBootstrapService bootstrapService;
        [Inject] private INpcLifeService npcLifeService;
    
        [SerializeField] private int weight;
    
        public Transform Transform => transform;

        public int Weight => weight;

        private void Awake()
        {
            Initialize();
        }

        private async UniTask Initialize()
        {
            await bootstrapService.BootstrapTask;
        
            npcLifeService.RegisterInterestPoint(this);
        }
    }
}