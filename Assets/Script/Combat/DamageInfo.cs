using UnityEngine;

namespace DemonCastle.Combat
{
    public readonly struct DamageInfo
    {
        public int Amount { get; }
        public Vector2 Direction { get; }
        public float Knockback { get; }
        public DamageInfo(int amount, Vector2 direction = default, float knockback = 0f)
        {
            Amount = amount;
            Direction = direction.normalized;
            Knockback = knockback;
        }
    }
}

