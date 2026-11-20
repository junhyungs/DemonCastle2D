using System;

namespace DemonCastle.Weapons
{
    public interface IWeaponMotion
    {
        event Action Hit;
        event Action ComboWindowOpened;
        event Action Ended;
        void Play(int step);
        void Stop();
    }
}

