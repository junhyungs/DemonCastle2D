using System;

namespace DemonCastle.Player
{
    public class PlayerStats
    {
        private float _moveSpeed;
        private float _rollSpeed;
        private float _rollDuration;
        private float _rollCooldown;

        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = RequirePositive(value, nameof(MoveSpeed));
        }

        public float RollSpeed
        {
            get => _rollSpeed;
            set => _rollSpeed = RequirePositive(value, nameof(RollSpeed));
        }

        public float RollDuration
        {
            get => _rollDuration;
            set => _rollDuration = RequirePositive(value, nameof(RollDuration));
        }

        public float RollCooldown
        {
            get => _rollCooldown;
            set => _rollCooldown = RequireNotNegative(value, nameof(RollCooldown));
        }

        public PlayerStats(float moveSpeed, float rollSpeed, float rollDuration, float rollCooldown)
        {
            MoveSpeed = moveSpeed;
            RollSpeed = rollSpeed;
            RollDuration = rollDuration;
            RollCooldown = rollCooldown;
        }

        private static float RequirePositive(float value, string name)
        {
            if (value <= 0f) throw new ArgumentOutOfRangeException(name, value, $"{name}은(는) 0보다 커야 합니다.");
            return value;
        }

        private static float RequireNotNegative(float value, string name)
        {
            if (value < 0f) throw new ArgumentOutOfRangeException(name, value, $"{name}은(는) 0 이상이어야 합니다.");
            return value;
        }
    }
}

