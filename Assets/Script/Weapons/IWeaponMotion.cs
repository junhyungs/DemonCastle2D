using System;

namespace DemonCastle.Weapons
{
    public interface IWeaponMotion
    {
        event Action Hit;
        event Action Ended;
        void Play();
    }
}

