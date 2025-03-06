using System.Collections.Generic;
using Game.NpcSystem;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Action
{
    [CreateAssetMenu(menuName = "AI/Action/Generic/PatrolAction", fileName = "PatrolAction")]
    public class PatrolAction : MovementAction
    {
        [SerializeField] private int radius;
        [SerializeField] private int pointsCount;

        public override int GetCost(WorldState worldState)
        {
            return minimumCost;
        }

        public override ActionPerformResult Perform(NpcCharacter npcCharacter, WorldState worldState)
        {
            if (!worldState.GetEffect(WorldStateKeysEnum.IS_ON_PATROL))
            {
                worldState.PatrolOrigin = worldState.Position;
                worldState.SetEffect(WorldStateKeysEnum.IS_ON_PATROL, true);
                
                worldState.PatrolPoints = new Queue<Vector3>();
                
                for (var i = 0; i < pointsCount; i++)
                {
                    worldState.PatrolPoints.Enqueue(
                        RandomNavmeshLocation(worldState));
                }

                if (worldState.PatrolPoints.Count == 0)
                {
                    return ActionPerformResult.Completed;
                }
            }

            var isOnPoint = npcCharacter.NavigateTo(worldState.PatrolPoints.Peek(), stopDistance);
            if (isOnPoint)
            {
                worldState.PatrolPoints.Dequeue();
            }
            
            return worldState.PatrolPoints.Count == 0 ? ActionPerformResult.Completed : ActionPerformResult.Performing;
        }

        public override void Complete(NpcCharacter npcCharacter, WorldState worldState)
        {
            worldState.SetEffect(WorldStateKeysEnum.IS_ON_PATROL, false);
        }

        public override void Fail(NpcCharacter npcCharacter, WorldState worldState)
        {
            worldState.SetEffect(WorldStateKeysEnum.IS_ON_PATROL, false);
        }

        private Vector3 RandomNavmeshLocation(WorldState worldState)
        {
            var randomDirection = Random.insideUnitSphere * radius;
            randomDirection += worldState.PatrolOrigin;

            NavMeshHit hit;

            var finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
            }

            return finalPosition;
        }
    }
}