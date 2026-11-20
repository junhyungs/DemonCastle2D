using UnityEngine;
using System;

namespace DemonCastle.Player
{
    public sealed class Facing
    {
        // 조준이 거의 수직(|x| < 0.1, 약 ±5.7도)이면 좌우를 바꾸지 않는다. 경계에서 깜빡임 방지.
        public const float DeadZone = 0.1f;

        public int Sign { get; private set; } = 1;
        public bool IsLocked { get; private set; }
        public Vector2 Forward => new Vector2(Sign, 0f);

        public event Action<int> Changed;

        public Vector2 ForwardFor(Vector2 aimDirection)
        {
            if (Mathf.Abs(aimDirection.x) < DeadZone) return Forward;
            return new Vector2(aimDirection.x > 0f ? 1f : -1f, 0f);
        }

        public void Follow(Vector2 aimDirection)
        {
            if (IsLocked) return;
            SetSign(ForwardFor(aimDirection));
        }

        public void Lock(Vector2 forward)
        {
            IsLocked = true;
            SetSign(forward);
        }

        public void Unlock() => IsLocked = false;

        private void SetSign(Vector2 forward)
        {
            if (forward.x == 0f) return;

            var sign = forward.x > 0f ? 1 : -1;
            if (sign == Sign) return;

            Sign = sign;
            Changed?.Invoke(Sign);
        }
    }
}

