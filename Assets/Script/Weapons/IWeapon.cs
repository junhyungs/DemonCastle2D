using DemonCastle.Core;
using System;
using UnityEngine;

namespace DemonCastle.Weapons
{
    public interface IWeapon : ITickable
    {
        bool IsReady { get; }
        bool TryAttack(Vector2 direction);
        event Action<Vector2> Attacked;
    }
}

