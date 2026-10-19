using UnityEngine;
using System;

namespace DemonCastle.Player
{
    public class PlayerMovement
    {
        private readonly float _speed;

        public PlayerMovement(float speed)
        {
            if (speed <= 0) throw new ArgumentOutOfRangeException(nameof(speed), speed, "이동 속도는 0보다 커야 합니다.");
            _speed = speed;
        }

        public Vector2 CalculateVelocity(Vector2 input)
        {
            Vector2 direction = Vector2.ClampMagnitude(input, 1f);
            return direction * _speed;
        }
    }
}

