using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AI.Action;
using AI.Goal;
using Cysharp.Threading.Tasks;
using Zenject;
using Debug = UnityEngine.Debug;

namespace Services.NpcLifeService
{
    public struct ActionPlaningTask
    {
        public Guid Id;
        public ActorGoal Goal;
        public ActorAction[] Actions;
        public WorldState WorldState;
        public Queue<ActorAction> Plan;

        public ActionPlaningTask(Guid id, ActorGoal goal, ActorAction[] actions, WorldState worldState)
        {
            Id = id;
            Goal = goal;
            Actions = actions;
            WorldState = worldState;
            Plan = null;
        }
    }
    
    public interface IActionPlaningService
    {
        public Action<ActionPlaningTask> OnPlanBuildComplete { get; set; }

        public void Bootstrap();
        
        public void EnqueueActionPlaningTask(Guid id, GoapAgent goapAgent, ActorGoal actorGoal);
    }
    
    public class ActionPlaningServiceInstaller : Installer<ActionPlaningServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ActionPlaningService>().AsSingle().NonLazy();
        }
    }

    public class ActionPlaningService : IActionPlaningService
    {
        public Action<ActionPlaningTask> OnPlanBuildComplete { get; set; }

        private Queue<ActionPlaningTask> planingTasks;

        private ActionPlaner planer;

        private CancellationTokenSource cts;

        private bool isProcessing;

        public void Bootstrap()
        {
            planer = new ActionPlaner();

            StartProcessing();
        }

        public void EnqueueActionPlaningTask(Guid id, GoapAgent goapAgent, ActorGoal actorGoal)
        {
            planingTasks ??= new Queue<ActionPlaningTask>();

            var planingTask = new ActionPlaningTask(
                id,
                actorGoal,
                goapAgent.Actions.ToArray(),
                goapAgent.WorldState
            );
            
            planingTasks.Enqueue(planingTask);
        }

        private async UniTask StartProcessing()
        {
            cts = new CancellationTokenSource();

            while (!cts.IsCancellationRequested)
            {
                await UniTask.Delay(2000);
                
                ProcessTasks();
            }
        }

        private void ProcessTasks()
        {
            if (planingTasks == null || planingTasks.Count == 0) return;
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var count = planingTasks.Count;

            isProcessing = true;
            
            while (planingTasks.Count > 0)
            {
                var task = planingTasks.Dequeue();
                task.Plan = planer.GetPlan(
                        task.Goal,
                        task.Actions.ToArray(),
                        task.WorldState
                    );
                
                OnPlanBuildComplete?.Invoke(task);
            }
            
            isProcessing = false;
            
            sw.Stop();
            TimeSpan timeTaken = sw.Elapsed;
            Debug.LogWarning($"[ActionPlaningService] ProcessTasks {count}: " + timeTaken.ToString(@"m\:ss\.ffffff")); 
        }
    }
}