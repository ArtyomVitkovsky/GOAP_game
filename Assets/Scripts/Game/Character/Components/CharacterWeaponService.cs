using System.Collections.Generic;

namespace Game.Character.Components
{
    public class CharacterWeaponService : ICharacterWeaponService
    {
        public List<IWeapon> Weapons { get; }
        public IWeapon CurrentWeapon { get; private set; }

        private int currentWeaponIndex = 0;

        public void Bootstrap()
        {
            CurrentWeapon = Weapons[currentWeaponIndex];
        }

        public void SelectNextWeapon()
        {
            currentWeaponIndex = currentWeaponIndex >= Weapons.Count - 1 ? 0 : currentWeaponIndex + 1;
            CurrentWeapon = Weapons[currentWeaponIndex];
        }

        public void SelectPreviousWeapon()
        {
            currentWeaponIndex = currentWeaponIndex == 0 ? Weapons.Count - 1 : currentWeaponIndex - 1;
            CurrentWeapon = Weapons[currentWeaponIndex];
        }
    }
}