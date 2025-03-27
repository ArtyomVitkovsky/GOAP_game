using UnityEngine;

namespace Game.Character.Installers
{
    [CreateAssetMenu(menuName = "Configs/Character/Interaction/InteractionSetup", fileName = "CharacterInteractionSetup")]
    public class CharacterInteractionSetup : ScriptableObject
    {
        public LayerMask layerMask;
        public float radius;
    }
}