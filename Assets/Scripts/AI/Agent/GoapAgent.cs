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

public class GoapAgent
{
    private IActionPlaningService ActionPlaningService;
    
    private List<Goal> Goals;
    
    private Goal CurrentGoal;
    private Queue<ActorAction> CurrentPlan;
    
    private ActorAction CurrentAction;

    public List<ActorAction> Actions;
    public WorldState WorldState;
    public NpcCharacter Character;
    
    private Guid planingTaskId;

    public GoapAgent(NpcCharacter character, WorldState worldState, IActionPlaningService actionPlaningService)
    {
        ActionPlaningService = actionPlaningService;
        Character = character;
        WorldState = worldState;

        WorldState.OnWorldStateChanged += ReBuild;
    }

    public void AddGoals(Goal[] goals)
    {
        Goals ??= new List<Goal>();
        Goals.AddRange(goals);
    }

    public void RemoveGoal(Goal goal)
    {
        if (Goals != null && Goals.Contains(goal))
        {
            Goals.Remove(goal);
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

    public virtual void ReBuild()
    {
        if (Goals == null || Goals.Count == 0) return;
        
        var goal = GetMostImportantGoal();

        CurrentGoal = goal;
        BuildPlanFor(CurrentGoal);
    }

    protected virtual void BuildPlanFor(Goal goal)
    {
        ActionPlaningService.OnPlanBuildComplete += OnPlanBuildComplete;
        planingTaskId = Guid.NewGuid();
        ActionPlaningService.EnqueueActionPlaningTask(planingTaskId, this, goal);
    }

    protected virtual void OnPlanBuildComplete(ActionPlaningTask planingTask)
    {
        if (planingTaskId == planingTask.Id)
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

    protected virtual void ProcessAction(ActorAction action)
    {
        switch (action.Perform(Character, WorldState))
        {
            case ActionPerformResult.Performing:
            {
                break;
            }
            case ActionPerformResult.Completed:
            {
                CurrentAction.Complete(Character, WorldState);
                CurrentPlan.Dequeue();
                break;
            }
            case ActionPerformResult.Failed:
            {
                CurrentAction.Fail(Character, WorldState);
                CurrentPlan.Dequeue();
                break;
            }
        }
    }

    private Goal GetMostImportantGoal()
    {
        Goal mostImportantGoal = null;

        foreach (var goal in Goals)
        {
            if (!goal.IsValid(WorldState)) continue;

            if (!mostImportantGoal || 
                goal.Priority(WorldState) > mostImportantGoal.Priority(WorldState))
            {
                mostImportantGoal = goal;
            }
        }

        return mostImportantGoal;
    }
}
