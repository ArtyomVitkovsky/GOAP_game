using UnityEngine;

namespace AI.Action
{
    [CreateAssetMenu(menuName = "AI/Action/ActionsSet", fileName = "ActionsSet")]
    public class ActionsSet : ScriptableObject
    {
        [SerializeField] private ActorAction[] baseActions;
        [SerializeField] private ActorAction[] subordinateActions;

        public ActorAction[] BaseActions => baseActions;
        public ActorAction[] SubordinateActions => subordinateActions;
    }
}