using UnityEngine;
using System;

namespace DemonCastle.Player
{
    public sealed class Facing
    {
        public const float DeadZone = 0.1f;

        public int Sign { get; private set; } = 1;
        public Vector2 Forward => new Vector2(Sign, 0f);

        public event Action<int> Changed;

        public void Follow(Vector2 aimDirection)
        {
            if (Mathf.Abs(aimDirection.x) < DeadZone) return;

            var sign = aimDirection.x > 0f ? 1 : -1;
            if (sign == Sign) return;

            Sign = sign;
            Changed?.Invoke(sign);
        }
    }
}

