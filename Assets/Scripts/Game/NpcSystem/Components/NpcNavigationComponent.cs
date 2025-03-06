using System;
using System.Collections.Generic;
using System.Linq;
using Game.Car;
using Game.Car.Components;
using Services.TickableService;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.NpcSystem.Components
{
    [Serializable]
    public class NavMeshAgentSetup
    {
        public float radius;
        public float height;
    }
    
    public static class AgentTypeID
    {
        public static int GetAgentTypeIDByName(string agentTypeName)
        {
            int count = NavMesh.GetSettingsCount();
            string[] agentTypeNames = new string[count + 2];
            for (var i = 0; i < count; i++)
            {
                int id = NavMesh.GetSettingsByIndex(i).agentTypeID;
                string name = NavMesh.GetSettingsNameFromID(id);
                if(name == agentTypeName)
                {
                    return id;
                }
            }
            return -1;
        }
    }
    
    public class NpcNavigationComponent
    {
        [Inject] private NavMeshAgent navMeshAgent;

        private Vector3 target;

        private TickableEntity tickableEntity;

        public NpcVehicleControlComponent VehicleControl;

        public NavMeshAgent NavMeshAgent { get; private set; }

        public Vector3 Target => target;
        
        public float StopDistance { get; private set; }
        
        public void Initialize()
        {
            NavMeshAgent = navMeshAgent;
        }

        public void SetActive(bool isActive)
        {
            NavMeshAgent.enabled = isActive;
        }
        
        public bool NavigateTo(Vector3 point, float stopDistance)
        {
            if (!NavMeshAgent.enabled)
            {
                return false;
            }
            
            StopDistance = stopDistance;
            
            var distance = (NavMeshAgent.transform.position - Target).sqrMagnitude;
            if (Target != point)
            {
                target = point;
                NavMeshAgent.SetDestination(Target);
            }
            else if (distance < stopDistance)
            {
                return true;
            }
        
            return false;
        }
    }
}