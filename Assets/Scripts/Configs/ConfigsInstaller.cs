using AI.Action;
using AI.Goal;
using Configs.PlayerConfig;
using Configs.Transmission;
using Game.Car.Installers;
using Game.NpcSystem;
using Services.ReputationService;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/Installers/ConfigsInstaller", fileName = "ConfigsInstaller")]
    public class ConfigsInstaller : ScriptableObjectInstaller
    {
        [Header("Player")]
        [SerializeField] private PlayerMovementBaseStats playerMovementBaseStats;
        [SerializeField] private VehicleMovementBaseStats vehicleMovementBaseStats;
        
        [Header("Vehicle")]
        [SerializeField] private TransmissionSettingsPanelsConfigsSet transmissionSettingsPanelsConfigsSet;

        [Header("Fractions")] 
        [SerializeField] private ReputationSetup reputationSetup;
        [SerializeField] private FractionReputationSetupSet fractionReputationSetupSet;
        
        [Header("AI")]
        [SerializeField] private GoalsSet goalsSet;
        [SerializeField] private ActionsSet actionsSet;


        public override void InstallBindings()
        {
            Container.BindInstance(transmissionSettingsPanelsConfigsSet);
            
            Container.BindInstance(playerMovementBaseStats);
            Container.BindInstance(vehicleMovementBaseStats);
            
            Container.BindInstance(reputationSetup);
            Container.BindInstance(fractionReputationSetupSet);
            
            Container.BindInstance(goalsSet);
            Container.BindInstance(actionsSet);
        }
    }
}
