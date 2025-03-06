using System.Collections.Generic;

namespace Game.Character.Components
{
    public interface ICharacterWeaponService
    {
        public List<IWeapon> Weapons { get; }

        public IWeapon CurrentWeapon { get; }

        public void Bootstrap();

        public void SelectNextWeapon();

        public void SelectPreviousWeapon();
    }
}