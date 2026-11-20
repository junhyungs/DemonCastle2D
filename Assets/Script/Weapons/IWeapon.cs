using DemonCastle.Core;
using UnityEngine;

namespace DemonCastle.Weapons
{
    public interface IWeapon : ITickable
    {
        bool IsReady { get; }
        bool IsAttacking { get; }
        Vector2 AttackDirection { get; }
        float MoveMultiplier { get; }

        bool TryAttack(Vector2 direction);
        void Cancel();
    }
}

