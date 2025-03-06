using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Car;
using Services;
using Services.NpcLifeService;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.NpcSystem
{
    [Serializable]
    public class NpcSpawnPoint
    {
        [SerializeField] private Transform transform;
        [SerializeField] private int spawnRadius;
    
        public Transform Transform => transform;
    
        public int SpawnRadius => spawnRadius;
    
        public Vector3 GetRandomPoint()
        {
            var randomX = Random.Range(transform.position.x - spawnRadius, transform.position.x + spawnRadius);
            var randomZ = Random.Range(transform.position.z - spawnRadius, transform.position.z + spawnRadius);
            var position = new Vector3(randomX, transform.position.y, randomZ);
        
            return position;
        }
    }

    public class NpcCampPoint : MonoBehaviour, IInterestPoint
    {
        [Inject] private DiContainer diContainer;
        [Inject] private IBootstrapService bootstrapService;
        [Inject] private INpcLifeService npcLifeService;

        [Header("Fraction")] 
        [SerializeField] private FractionSetup fractionSetup;
    
        [Header("Camp")]
        [SerializeField] private NpcCharacter npcPrefab;
        [SerializeField] private List<NpcSpawnPoint> spawnPoints;
        [SerializeField] private int maxPopulation;
        
        [Header("Vehicles")]
        [SerializeField] private List<VehicleSystem> vehicles;
    
        [Header("Interest Point")]
        [SerializeField] private int weight;

        private List<NpcCharacter> citizens;

        public Transform Transform => transform;
        public int Weight => weight;
    
        private void Awake()
        {
            Initialize();
        }
    
        private async UniTask Initialize()
        {
            await bootstrapService.BootstrapTask;
        
            citizens = new List<NpcCharacter>();
        
            for (int i = 0; i < maxPopulation; i++)
            {
                SpawnNpc();
            }
        
            npcLifeService.RegisterCampPoint(this);
            npcLifeService.RegisterInterestPoint(this);
        }

        private void SpawnNpc()
        {
            var spawnPointIndex = Random.Range(0, spawnPoints.Count);

            var spawnPoint = spawnPoints[spawnPointIndex];
            var position = spawnPoint.GetRandomPoint();

            var newCitizen =
                diContainer.InstantiatePrefabForComponent<NpcCharacter>(
                    npcPrefab, position, Quaternion.identity, spawnPoint.Transform
                );
        
            newCitizen.Initialize(fractionSetup.Fraction);
            citizens.Add(newCitizen);
        }

        public List<NpcCharacter> GetRandomGroup()
        {
            var groupSize = (int)(citizens.Count * 0.3f);
            groupSize = Math.Clamp(groupSize, 0, vehicles.Count);

            var group = new List<NpcCharacter>(groupSize);
            var usedIndices = new List<int>(groupSize);
        
            for (int i = 0; i < groupSize; i++)
            {
                var randomIndex = Random.Range(0, citizens.Count);

                while (usedIndices.Contains(randomIndex))
                {
                    randomIndex = Random.Range(0, citizens.Count);
                }
            
                usedIndices.Add(randomIndex);
            
                var groupMember = citizens[randomIndex];
                group.Add(groupMember);
            }

            return group;
        }
    }
}