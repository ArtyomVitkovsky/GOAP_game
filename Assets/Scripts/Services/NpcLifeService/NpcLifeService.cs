using System.Collections.Generic;
using AI.Action;
using AI.Goal;
using AI.Goal.Generic;
using Cysharp.Threading.Tasks;
using Game.NpcSystem;
using UnityEngine;
using Zenject;

namespace Services.NpcLifeService
{
    public class GroupGoapAgent : GoapAgent
    {
        private GoalsSet goalsSet;
        private ActionsSet actionsSet;

        private List<GoapAgent> members;

        public GroupGoapAgent(
            NpcCharacter character,
            WorldState worldState,
            IActionPlaningService actionPlaningService,
            GoalsSet goalsSet,
            ActionsSet actionsSet,
            int capacity)
            : base(character, worldState, actionPlaningService)
        {
            this.goalsSet = goalsSet;
            this.actionsSet = actionsSet;
            
            members = new List<GoapAgent>(capacity);
        }

        public void AddMember(GoapAgent member)
        {
            members.Add(member);
            member.AddGoals(goalsSet.SubordinateGoals);
            member.AddActions(actionsSet.SubordinateActions);
            
            member.WorldState.SetEffect(WorldStateKeysEnum.IS_HAS_LEADER, true);
            
            WorldState.OnWorldStateChanged += ReBuild;
        }

        public override void ReBuild()
        {
            base.ReBuild();

            for (var i = 0; i < members.Count; i++)
            {
                members[i].ReBuild();
            }
        }

        protected override void BuildPlanFor(Goal goal)
        {
            base.BuildPlanFor(goal);
        }

        protected override void OnPlanBuildComplete(ActionPlaningTask planingTask)
        {
            base.OnPlanBuildComplete(planingTask);
        }

        public override void ProcessPlan()
        {
            base.ProcessPlan();

            for (var i = 0; i < members.Count; i++)
            {
                members[i].ProcessPlan();
            }
        }

        protected override void ProcessAction(ActorAction action)
        {
            base.ProcessAction(action);
        }
    }

    public class NpcLifeService : INpcLifeService
    {
        [Inject] private IActionPlaningService actionPlaningService;

        [Inject] private GoalsSet goalsSet;
        [Inject] private ActionsSet actionsSet;

        private List<GoapAgent> goapAgents;

        public List<NpcCampPoint> camps;

        public List<IInterestPoint> interestPoints;

        public void Bootstrap()
        {
            camps = new List<NpcCampPoint>();
            interestPoints = new List<IInterestPoint>();
        }

        public void RegisterCampPoint(NpcCampPoint campPoint)
        {
            camps.Add(campPoint);
            CreateExplorationGroupFor(campPoint, Random.Range(0f, 10f));
        }

        public void RegisterInterestPoint(IInterestPoint interestPoint)
        {
            interestPoints.Add(interestPoint);
        }

        private async UniTask CreateExplorationGroupFor(NpcCampPoint campPoint, float delay)
        {
            await UniTask.Delay((int)(delay * 1000f));

            var group = campPoint.GetRandomGroup();
            if (group.Count == 0) return;

            var points = interestPoints.FindAll(p => p != campPoint as IInterestPoint);
            var randomInterestPoint = points[Random.Range(0, points.Count)];

            var leader = group[0];
            var groupAgent = new GroupGoapAgent(
                leader,
                leader.WorldState,
                actionPlaningService,
                goalsSet,
                actionsSet,
                group.Count);

            for (var i = 0; i < group.Count; i++)
            {
                var member = group[i];
                groupAgent.AddMember(member.GoapAgent);
            }

            groupAgent.AddActions(actionsSet.BaseActions);
            groupAgent.AddGoals(goalsSet.BaseGoals);
            groupAgent.WorldState.Target = randomInterestPoint.Transform.position;
            groupAgent.WorldState.SetEffect(WorldStateKeys.IS_HAS_TARGET, true);
        }
    }
}