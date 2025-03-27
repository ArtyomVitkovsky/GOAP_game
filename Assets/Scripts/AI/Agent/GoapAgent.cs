using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.Action;
using AI.Goal;
using Game.NpcSystem;
using Services.NpcLifeService;
using UnityEngine;
using Zenject;

// public class AgentComponentInstaller : Installer<AgentComponentInstaller>
// {
//     public override void InstallBindings()
//     {
//         Container.BindInterfacesAndSelfTo<AgentComponent>().AsSingle().NonLazy();
//     }
// }

public class GoapAgent : IGoapAgent<NpcCharacter>
{
    private IActionPlaningService ActionPlaningService;

    public GoapAgent(NpcCharacter character, WorldState worldState, IActionPlaningService actionPlaningService)
    {
        ActionPlaningService = actionPlaningService;
        Context = character;
        WorldState = worldState;
        
        ActionPlaningService.OnPlanBuildComplete += OnPlanBuildComplete;
    }

    public WorldState WorldState { get; protected set; }
    
    public NpcCharacter Context { get; protected set; }
    public List<ActorGoal> Goals { get; protected set; }
    public List<ActorAction> Actions { get; protected set; }
    public ActorGoal CurrentGoal { get; protected set; }
    public Queue<ActorAction> CurrentPlan { get; protected set; }
    public ActorAction CurrentAction { get; protected set; }

    public Guid PlaningTaskId { get; protected set; }

    public void AddGoals(ActorGoal[] goals)
    {
        Goals ??= new List<ActorGoal>();
        Goals.AddRange(goals);
    }

    public void RemoveGoal(ActorGoal actorGoal)
    {
        if (Goals != null && Goals.Contains(actorGoal))
        {
            Goals.Remove(actorGoal);
        }
    }

    public void AddActions(ActorAction[] actions)
    {
        Actions ??= new List<ActorAction>();
        Actions.AddRange(actions);
    }
    
    public void RemoveAction(ActorAction action)
    {
        if (Actions != null && Actions.Contains(action))
        {
            Actions.Remove(action);
        }
    }

    public void StartWorldStateListening()
    {
        WorldState.OnWorldStateChanged += () => { ReBuild(false); };
    }

    public virtual void ReBuild(bool force)
    {
        if (Goals == null || Goals.Count == 0 && !force) return;
        
        var goal = GetMostImportantGoal();

        BuildPlanFor(goal);
    }

    public void BuildPlanFor(ActorGoal goal)
    {
        CurrentGoal = goal;

        PlaningTaskId = Guid.NewGuid();
        ActionPlaningService.EnqueueActionPlaningTask(PlaningTaskId, this, goal);
    }

    public void OnPlanBuildComplete(ActionPlaningTask planingTask)
    {
        if (PlaningTaskId == planingTask.Id)
        {
            CurrentPlan = planingTask.Plan;
        }
    }

    public virtual void ProcessPlan()
    {
        if (CurrentPlan == null || CurrentPlan.Count == 0) return;

        CurrentAction = CurrentPlan.Peek();
        ProcessAction(CurrentAction);
    }

    public void ProcessAction(ActorAction action)
    {
        switch (action.Perform(Context, WorldState))
        {
            case ActionPerformResult.Performing:
            {
                break;
            }
            case ActionPerformResult.Completed:
            {
                CurrentAction.Complete(Context, WorldState);
                CurrentPlan.Dequeue();
                break;
            }
            case ActionPerformResult.Failed:
            {
                CurrentAction.Fail(Context, WorldState);
                CurrentPlan.Dequeue();
                break;
            }
        }
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
