using System;
using System.Collections.Generic;
using System.Linq;
using Game.NpcSystem;
using UnityEngine;

public class WorldState
{
    public Action OnWorldStateChanged;
    
    public Vector3 Position;
    public int Health;
    public int MaxHealth;
    
    public Fraction Fraction;
 
    public Vector3 Target;
    public Vector3 ClosestVehicle;
    
    public List<Vector3> VehiclePositions;

    public Vector3 PatrolOrigin;
    public Queue<Vector3> PatrolPoints;
    
    private Dictionary<string, bool> effects = new Dictionary<string, bool> (15)
    {
        { WorldStateKeys.IS_ALIVE           , true  }, // 1
        { WorldStateKeys.IS_NEAR_TARGET     , false }, // 2
        { WorldStateKeys.IS_AT_TARGET       , false }, // 3
        { WorldStateKeys.IS_HAS_TARGET      , false }, // 4
        { WorldStateKeys.IS_ENEMY_NEARBY    , false }, // 5
        { WorldStateKeys.IS_SEE_ENEMY       , false }, // 6
        { WorldStateKeys.IS_AT_ENEMY        , false }, // 7
        { WorldStateKeys.IS_ENEMY_ALIVE     , false }, // 8
        { WorldStateKeys.IS_DAMAGED         , false }, // 9
        { WorldStateKeys.IS_HAS_HEAL        , false }, // 10
        { WorldStateKeys.IS_CAN_HEAL        , true  }, // 11
        { WorldStateKeys.IS_HAS_VEHICLE     , false }, // 12
        { WorldStateKeys.IS_SEE_VEHICLE     , false }, // 13
        { WorldStateKeys.IS_CAN_USE_VEHICLE , false }, // 14
        { WorldStateKeys.IS_ON_PATROL       , false }, // 15
    };

    public bool GetEffect(string key)
    {
        return effects[key];
    }
    
    public bool GetEffect(WorldStateKeysEnum key)
    {
        return effects[WorldStateKeys.TypeToKeys[key]];
    }
    
    public Dictionary<string, bool> GetEffects()
    {
        return effects.ToDictionary(entry => entry.Key, entry => entry.Value);
    }

    public void SetEffect(string key, bool value)
    {
        // if (effects[key] == value) return;
        
        effects[key] = value;
        OnWorldStateChanged?.Invoke();
    }
    
    public void SetEffect(WorldStateKeysEnum key, bool value)
    {
        // if (effects[key] == value) return;
        
        effects[WorldStateKeys.TypeToKeys[key]] = value;
        OnWorldStateChanged?.Invoke();
    }

    public void GetClosestVehicle(out Vector3 closest, out float distance)
    {
        closest = Vector3.positiveInfinity;
        distance = 1000000;
        
        if (VehiclePositions == null || VehiclePositions.Count == 0)
        {
            return;
        }

        var closestDistanceSqr = Mathf.Infinity;
        for (var i = 0; i < VehiclePositions.Count; i++)
        {
            var vehicle = VehiclePositions[i];
            var directionToTarget = vehicle - Position;
            var dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closest = vehicle;
            }
        }

        distance = (Position - closest).sqrMagnitude;
    }
}
