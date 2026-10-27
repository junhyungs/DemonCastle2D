using UnityEngine;

namespace DemonCastle.Player
{
    public static class PlayerMovement
    {
        public static Vector2 CalculateVelocity(Vector2 input, float speed)
        {
            Vector2 direction = Vector2.ClampMagnitude(input, 1f);
            return direction * speed;
        }
    }
}

