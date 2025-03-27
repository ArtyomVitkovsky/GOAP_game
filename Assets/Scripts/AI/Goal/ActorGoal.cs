using System.Collections.Generic;
using System.Linq;
using AI.Action;
using UnityEngine;

namespace AI.Goal
{
    public abstract class ActorGoal : ScriptableObject
    {
        [SerializeField] protected string id;
        
        [SerializeField] protected Fact[] desiredState;
        [SerializeField] protected Fact[] validationParameters;

        [Header("Priority")]
        [SerializeField] protected int defaultPriority;
        [SerializeField] protected int minimumPriority;
        [SerializeField] protected int maximumPriority;

        public string Id => id;
        
        public Dictionary<string, bool> DesiredState
        {
            get
            {
                if (desiredState == null || desiredState.Length == 0)
                {
                    return new Dictionary<string, bool>(0);
                }
                
                return desiredState.ToDictionary(c => WorldStateKeys.TypeToKeys[c.Key], c => c.Value);
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
    
        public abstract int Priority(WorldState worldState);

        public abstract Dictionary<string, bool> GetDesiredState();
    }
}