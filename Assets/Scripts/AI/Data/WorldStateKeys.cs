using System.Collections.Generic;

public static class WorldStateKeys
{
    public const string IS_ALIVE = "IS_ALIVE";

    public const string IS_DAMAGED = "IS_DAMAGED";
    public const string IS_HAS_HEAL = "IS_HAS_HEAL";
    public const string IS_CAN_HEAL = "IS_CAN_HEAL";

    public const string IS_SEE_VEHICLE = "IS_SEE_VEHICLE";
    public const string IS_CAN_USE_VEHICLE = "IS_CAN_USE_VEHICLE";
    public const string IS_HAS_VEHICLE = "IS_HAS_VEHICLE";

    public const string IS_HAS_TARGET = "IS_HAS_TARGET";
    public const string IS_NEAR_TARGET = "IS_NEAR_TARGET";
    public const string IS_AT_TARGET = "IS_AT_TARGET";

    public const string IS_ENEMY_NEARBY = "IS_ENEMY_NEARBY";
    public const string IS_SEE_ENEMY = "IS_SEE_ENEMY";
    public const string IS_AT_ENEMY = "IS_AT_ENEMY";
    public const string IS_ENEMY_ALIVE = "IS_ENEMY_ALIVE";
    public const string IS_ON_PATROL = "IS_ON_PATROL";
    
    public const string IS_HAS_LEADER = "IS_HAS_LEADER";
    public const string IS_LEADER_ATTACKED = "IS_LEADER_ATTACKED";
    
    public static Dictionary<WorldStateKeysEnum, string> TypeToKeys = new Dictionary<WorldStateKeysEnum, string> (15)
    {
        { WorldStateKeysEnum.IS_ALIVE           , IS_ALIVE           }, // 1
        { WorldStateKeysEnum.IS_NEAR_TARGET     , IS_NEAR_TARGET     }, // 2
        { WorldStateKeysEnum.IS_AT_TARGET       , IS_AT_TARGET       }, // 3
        { WorldStateKeysEnum.IS_HAS_TARGET      , IS_HAS_TARGET      }, // 4
        { WorldStateKeysEnum.IS_ENEMY_NEARBY    , IS_ENEMY_NEARBY    }, // 5
        { WorldStateKeysEnum.IS_SEE_ENEMY       , IS_SEE_ENEMY       }, // 6
        { WorldStateKeysEnum.IS_AT_ENEMY        , IS_AT_ENEMY        }, // 7
        { WorldStateKeysEnum.IS_ENEMY_ALIVE     , IS_ENEMY_ALIVE     }, // 8
        { WorldStateKeysEnum.IS_DAMAGED         , IS_DAMAGED         }, // 9
        { WorldStateKeysEnum.IS_HAS_HEAL        , IS_HAS_HEAL        }, // 10
        { WorldStateKeysEnum.IS_CAN_HEAL        , IS_CAN_HEAL        }, // 11
        { WorldStateKeysEnum.IS_HAS_VEHICLE     , IS_HAS_VEHICLE     }, // 12
        { WorldStateKeysEnum.IS_SEE_VEHICLE     , IS_SEE_VEHICLE     }, // 13
        { WorldStateKeysEnum.IS_CAN_USE_VEHICLE , IS_CAN_USE_VEHICLE }, // 14
        { WorldStateKeysEnum.IS_ON_PATROL       , IS_ON_PATROL       }, // 15
        { WorldStateKeysEnum.IS_HAS_LEADER      , IS_HAS_LEADER      }, // 16
        { WorldStateKeysEnum.IS_LEADER_ATTACKED , IS_LEADER_ATTACKED }, // 17
    };
}

public enum WorldStateKeysEnum
{
    IS_ALIVE = 0,

    IS_DAMAGED = 2,
    IS_HAS_HEAL = 3,
    IS_CAN_HEAL = 4,

    IS_SEE_VEHICLE = 5,
    IS_CAN_USE_VEHICLE = 6,
    IS_HAS_VEHICLE = 7,

    IS_HAS_TARGET = 8,
    IS_NEAR_TARGET = 9,
    IS_AT_TARGET = 10,

    IS_ENEMY_NEARBY = 11,
    IS_SEE_ENEMY = 12,
    IS_AT_ENEMY = 13,
    IS_ENEMY_ALIVE = 14,
    IS_ON_PATROL = 15,
    
    IS_HAS_LEADER = 16,
    IS_LEADER_ATTACKED = 17
}