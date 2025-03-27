using System;
using System.Collections.Generic;
using System.Linq;
using AI.Action;
using AI.Goal;
using Game.NpcSystem;
using UnityEngine;

namespace Services.NpcLifeService
{
    public class GroupGoapContext : IGoapContext
    {
        public List<GoapAgent> Members;

        public GroupGoapContext()
        {
            Members = new List<GoapAgent>();
        }
    }

    public class GroupGoapAgent : IGoapAgent<GroupGoapContext>
    {
        private IActionPlaningService actionPlaningService;

        private HashSet<GoapAgent> agentsCompletedAction;
        private HashSet<GoapAgent> agentsCompletedPlan;
        
        private bool isPlanBuilded;

        public WorldState WorldState { get; protected set; }
        public GroupGoapContext Context { get; protected set; }
        public List<ActorGoal> Goals { get; protected set; }
        public List<ActorAction> Actions { get; protected set; }
        public ActorGoal CurrentGoal { get; protected set; }
        public Queue<ActorAction> CurrentPlan { get; protected set; }
        public ActorAction CurrentAction { get; protected set; }
        public Guid PlaningTaskId { get; protected set; }

        public GroupGoapAgent(IActionPlaningService actionPlaningService, GroupGoapContext context, WorldState worldState)
        {
            Context = context;
            WorldState = worldState;

            this.actionPlaningService = actionPlaningService;
            this.actionPlaningService.OnPlanBuildComplete += OnPlanBuildComplete;
            
            StartWorldStateListening();
        }
        
        public void SetEffectForGroup(WorldStateKeysEnum key, bool value)
        {
            for (int i = 0; i < Context.Members.Count; i++)
            {
                Context.Members[i].WorldState.SetEffect(key, value);
            }
        }

        private void OnMemberWorldStateChanged()
        {
            foreach (var effect in WorldState.GetEffects())
            {
                var membersAmountWithState = Context.Members.Count(m => m.WorldState.GetEffect(effect.Key));
                var averageMembersState = (float) membersAmountWithState / Context.Members.Count > 0.7f;
                
                WorldState.SetEffect(effect.Key, averageMembersState);
            }
        }

        public void AddGoals(ActorGoal[] goals)
        {
            Goals ??= new List<ActorGoal>();
            Goals.AddRange(goals);
            
            for (var i = 0; i < Context.Members.Count; i++)
            {
                Context.Members[i].AddGoals(goals);
            }
        }

        public void RemoveGoal(ActorGoal actorGoal)
        {
            Goals.Remove(actorGoal);

            for (var i = 0; i < Context.Members.Count; i++)
            {
                Context.Members[i].RemoveGoal(actorGoal);
            }
        }

        public void AddActions(ActorAction[] actions)
        {
            Actions ??= new List<ActorAction>();
            Actions.AddRange(actions);
            
            for (var i = 0; i < Context.Members.Count; i++)
            {
                Context.Members[i].AddActions(actions);
            }
        }

        public void RemoveAction(ActorAction action)
        {
            Actions.Remove(action);
            
            for (var i = 0; i < Context.Members.Count; i++)
            {
                Context.Members[i].RemoveAction(action);
            }
        }

        public void StartWorldStateListening()
        {
            WorldState.OnWorldStateChanged += () => { ReBuild(false); };

            for (var i = 0; i < Context.Members.Count; i++)
            {
                Context.Members[i].WorldState.OnWorldStateChanged += OnMemberWorldStateChanged;
            }
        }

        public void ReBuild(bool force)
        {
            if (Goals == null || Goals.Count == 0) return;
            
            var goal = GetMostImportantGoal();
            
            if (CurrentGoal == goal && !force) return;
            
            isPlanBuilded = false;

            CurrentGoal = goal;
            BuildPlanFor(CurrentGoal);
        }

        public void BuildPlanFor(ActorGoal goal)
        {
            for (var i = 0; i < Context.Members.Count; i++)
            {
                Context.Members[i].BuildPlanFor(goal);
            }
        }

        public void OnPlanBuildComplete(ActionPlaningTask planingTask)
        {
            if (isPlanBuilded || planingTask.Goal != CurrentGoal) return;
            
            if (!Context.Members.Any(m => m.CurrentPlan == null || m.CurrentPlan.Count == 0))
            {
                agentsCompletedAction = new HashSet<GoapAgent>();
                agentsCompletedPlan = new HashSet<GoapAgent>();
                isPlanBuilded = true;
            }
        }

        public void ProcessPlan()
        {
            if (!isPlanBuilded) return;
            
            if (agentsCompletedAction.Count == Context.Members.Count)
            {
                agentsCompletedAction.Clear();
                for (var i = 0; i < Context.Members.Count; i++)
                {
                    var action = Context.Members[i].CurrentPlan.Dequeue();
                    action.Complete(Context.Members[i].Context, Context.Members[i].WorldState);
                    
                    if (Context.Members[i].CurrentPlan.Count == 0)
                    {
                        agentsCompletedPlan.Add(Context.Members[i]);
                    }
                }
            }
            
            if (agentsCompletedPlan.Count == Context.Members.Count)
            {
                agentsCompletedPlan.Clear();
                ReBuild(true);
                return;
            } 

            for (var i = 0; i < Context.Members.Count; i++)
            {
                if (agentsCompletedAction.Contains(Context.Members[i]) || Context.Members[i].CurrentPlan.Count == 0)
                {
                    continue;
                }
                
                var result = Context.Members[i]
                    .CurrentPlan
                    .Peek()
                    .Perform(Context.Members[i].Context, Context.Members[i].WorldState);

                if (result == ActionPerformResult.Failed)
                {
                    Context.Members[i]
                        .CurrentPlan
                        .Peek()
                        .Fail(Context.Members[i].Context, Context.Members[i].WorldState);;
                    Context.Members[i].ReBuild(true);
                }

                if (result == ActionPerformResult.Completed)
                {
                    agentsCompletedAction.Add(Context.Members[i]);
                }
            }
        }

        public void ProcessAction(ActorAction action)
        {
        }

        public ActorGoal GetMostImportantGoal()
        {
            ActorGoal mostImportantActorGoal = null;

            foreach (var goal in Goals)
            {
                if (!goal.IsValid(WorldState)) continue;

                if (!mostImportantActorGoal || 
                    goal.Priority(WorldState) > mostImportantActorGoal.Priority(WorldState))
                {
                    mostImportantActorGoal = goal;
                }
            }

            return mostImportantActorGoal;
        }
    }
}