using DemonCastle.Core;
using UnityEngine;

namespace DemonCastle.Weapons
{
    public interface IWeapon : ITickable
    {
        bool IsReady { get; }
        bool TryAttack(Vector2 direction);        
    }
}

