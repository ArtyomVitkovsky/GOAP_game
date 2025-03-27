using System.Collections.Generic;
using AI.Action;
using AI.Goal;
using Cysharp.Threading.Tasks;
using Game.NpcSystem;
using Services.TickableService;
using UnityEngine;
using Zenject;

namespace Services.NpcLifeService
{
    public class NpcLifeService : INpcLifeService
    {
        [Inject] private ITickableService tickableService;
        [Inject] private IActionPlaningService actionPlaningService;

        [Inject] private GoalsSet goalsSet;
        [Inject] private ActionsSet actionsSet;

        private List<IGoapAgent<IGoapContext>> goapAgents;

        private TickableEntity tickableEntity;

        private List<NpcCampPoint> camps;

        private List<IInterestPoint> interestPoints;
        

        public void Bootstrap()
        {
            camps = new List<NpcCampPoint>();
            interestPoints = new List<IInterestPoint>();
            goapAgents = new List<IGoapAgent<IGoapContext>>();
            
            tickableEntity = new TickableEntity(ProcessAgents);
            tickableService.AddFixedUpdateTickable(tickableEntity);
        }

        private void ProcessAgents()
        {
            for (int i = 0; i < goapAgents.Count; i++)
            {
                goapAgents[i].ProcessPlan();;
            }
        }

        public void RegisterCampPoint(NpcCampPoint campPoint)
        {
            camps.Add(campPoint);
            CreateExplorationGroupFor(campPoint, Random.Range(0f, 3f));
            SetupFreeCitizens(campPoint);
        }

        public void RegisterInterestPoint(IInterestPoint interestPoint)
        {
            interestPoints.Add(interestPoint);
        }

        private void SetupFreeCitizens(NpcCampPoint campPoint)
        {
            var freeCitizens = campPoint.GetFreeCitizens();
            for (int i = 0; i < freeCitizens.Count; i++)
            {
                freeCitizens[i].GoapAgent.AddGoals(goalsSet.BaseGoals);
                freeCitizens[i].GoapAgent.AddActions(actionsSet.BaseActions);
                freeCitizens[i].GoapAgent.ReBuild(false);
            }
        }

        private async UniTask CreateExplorationGroupFor(NpcCampPoint campPoint, float delay)
        {
            var group = campPoint.GetRandomGroup();
            if (group.Count == 0) return;
            
            await UniTask.Delay((int)(delay * 1000f));

            var points = interestPoints.FindAll(p => p != campPoint as IInterestPoint);
            var randomInterestPoint = points[Random.Range(0, points.Count)];

            var groupAgent = CreateGroupAgent(group);

            goapAgents.Add(groupAgent);

            var positionOffset = new Vector3(10, 0, 10);
            groupAgent.WorldState.Target = randomInterestPoint.Transform.position;
            
            for (var i = 0; i < group.Count; i++)
            {
                var offset = positionOffset * (i + 1);
                group[i].WorldState.Target = randomInterestPoint.Transform.position + offset;
            }
            
            groupAgent.SetEffectForGroup(WorldStateKeysEnum.IS_HAS_TARGET, true);
        }

        private GroupGoapAgent CreateGroupAgent(List<NpcCharacter> group)
        {
            var groupContext = new GroupGoapContext();
            
            for (var i = 0; i < group.Count; i++)
            {
                var member = group[i];
                groupContext.Members.Add(member.GoapAgent);
            }
            
            var groupAgent = new GroupGoapAgent(actionPlaningService, groupContext, new WorldState());

            groupAgent.AddActions(actionsSet.BaseActions);
            groupAgent.AddGoals(goalsSet.BaseGoals);

            return groupAgent;
        }
    }
}