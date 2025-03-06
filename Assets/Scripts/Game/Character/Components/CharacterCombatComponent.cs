using Services.PlayerControlService;
using Zenject;

namespace Game.Character.Components
{
    public class CharacterCombatComponent
    {
        [Inject] private IPlayerControlService playerControlService;
    }
}