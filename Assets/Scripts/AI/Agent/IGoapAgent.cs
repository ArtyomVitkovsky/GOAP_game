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

public interface IGoapContext
{
    
}

public interface IGoapAgent<out T> where T : IGoapContext
{
    public WorldState WorldState { get; }
    
    public T Context { get; }
    
    public List<ActorGoal> Goals { get; }

    public List<ActorAction> Actions { get; }
    
    public ActorGoal CurrentGoal { get; }
    
    public Queue<ActorAction> CurrentPlan { get; }

    public ActorAction CurrentAction { get; }
    
    
    public Guid PlaningTaskId { get; }

    
    public void AddGoals(ActorGoal[] goals);

    public void RemoveGoal(ActorGoal actorGoal);

    public void AddActions(ActorAction[] actions);

    public void RemoveAction(ActorAction action);

    public void StartWorldStateListening();
    
    public void ReBuild(bool force);
    
    public void ProcessPlan();
    
    public void BuildPlanFor(ActorGoal goal);

    public void OnPlanBuildComplete(ActionPlaningTask planingTask);

    public void ProcessAction(ActorAction action);

    public ActorGoal GetMostImportantGoal();
}
