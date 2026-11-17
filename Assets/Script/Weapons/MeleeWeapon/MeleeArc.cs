using UnityEngine;

namespace DemonCastle.Weapons
{
    public static class MeleeArc
    {
        private const float MinDistance = 0.0001f;

        public static bool Contains(Vector2 origin, Vector2 direction, Vector2 point, float range, float arcDegrees)
        {
            var toPoint = point - origin;
            var distance = toPoint.magnitude;

            if (distance > range) return false;
            // 몸이 겹쳐 있으면 방향을 따질 수 없으니 맞은 것으로 본다.
            if (distance < MinDistance) return true;

            return Vector2.Angle(direction, toPoint) <= arcDegrees * 0.5f;
        }
    }
}

