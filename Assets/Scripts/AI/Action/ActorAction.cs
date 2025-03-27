using System;
using System.Collections.Generic;
using System.Linq;
using Game.NpcSystem;
using UnityEngine;

namespace AI.Action
{
    [Serializable]
    public class Fact
    {
        public WorldStateKeysEnum Key;
        public bool Value;
    }

    public enum ActionPerformResult
    {
        Performing = 0,
        Completed = 1,
        Failed = 2
    }
    
    public abstract class ActorAction : ScriptableObject
    {
        [SerializeField] protected string id;
        
        [SerializeField] protected Fact[] preconditions;
        [SerializeField] protected Fact[] effects;
        [SerializeField] protected Fact[] validationParameters;
        
        [Header("Cost")]
        [SerializeField] protected float costMultiplier;
        [SerializeField] protected int minimumCost;
        [SerializeField] protected int maximumCost;

        public string Id => id;
        
        public Dictionary<string, bool> Preconditions
        {
            get
            {
                if (preconditions == null || preconditions.Length == 0)
                {
                    return new Dictionary<string, bool>(0);
                }
                
                return preconditions.ToDictionary(c => WorldStateKeys.TypeToKeys[c.Key], c => c.Value);
            }
        }
        
        public Dictionary<string, bool> Effects
        {
            get
            {
                if (effects == null || effects.Length == 0)
                {
                    return new Dictionary<string, bool>(0);
                }

                return effects.ToDictionary(c => WorldStateKeys.TypeToKeys[c.Key], c => c.Value);
            }
        }
        
        public bool IsValid(WorldState worldState)
        {
            if (validationParameters == null || validationParameters.Length == 0)
            {
                return true;
            }
            
            for (var i = 0; i < validationParameters.Length; i++)
            {
                if (worldState.GetEffect(validationParameters[i].Key) != validationParameters[i].Value)
                {
                    return false;
                }
            }

            return true;
        }

        public abstract int GetCost(WorldState worldState);

        public abstract ActionPerformResult Perform(NpcCharacter npcCharacter, WorldState worldState);
        public abstract void Complete(NpcCharacter npcCharacter, WorldState worldState);
        public abstract void Fail(NpcCharacter npcCharacter, WorldState worldState);
    }
}