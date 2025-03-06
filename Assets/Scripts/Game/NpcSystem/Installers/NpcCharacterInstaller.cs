using Game.Character.Installers;
using Game.NpcSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.NpcSystem.Installers
{
    public class NpcCharacterInstaller : MonoInstaller
    {
        public const string CHARACTER_INTERACTION_SETUP = "NPC/CHARACTER_INTERACTION_SETUP";
        public const string CHARACTER_INTERACTION_ORIGIN_POINT = "NPC/CHARACTER_INTERACTION_ORIGIN_POINT";
        public const string HEALTH_TEXT = "HEALTH_TEXT";
        public const string HEALTH_VALUE = "HEALTH_VALUE";
    
        [SerializeField] private CharacterInteractionSetup interactionSetup;
        [SerializeField] private Transform originPoint;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private int health;

        public override void InstallBindings()
        {
            Container.BindInstance(navMeshAgent);
            Container.BindInstance(health).WithId(HEALTH_VALUE);
            Container.BindInstance(healthText).WithId(HEALTH_TEXT);
            Container.BindInstance(interactionSetup).WithId(CHARACTER_INTERACTION_SETUP);
            Container.BindInstance(originPoint).WithId(CHARACTER_INTERACTION_ORIGIN_POINT);
        
            // AgentComponentInstaller.Install(Container);
            NpcNavigationComponentInstaller.Install(Container);
            NpcMoodComponentInstaller.Install(Container);
            NpcEncounterComponentInstaller.Install(Container);
            NpcInteractionComponentInstaller.Install(Container);
            NpcHealthComponentInstaller.Install(Container);
            NpcSensorsComponentInstaller.Install(Container);
        }
    }
}