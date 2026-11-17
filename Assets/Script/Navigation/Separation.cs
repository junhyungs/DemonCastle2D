using System.Collections.Generic;
using System;
using UnityEngine;

namespace DemonCastle.Navigation
{
    public static class Separation
    {
        private const float MinDistance = 0.0001f;

        public static Vector2 Calculate(Vector2 self, IReadOnlyList<Vector2> neighbors, float radius)
        {
            if(neighbors == null) throw new ArgumentNullException(nameof(neighbors));
            if(radius <= 0) throw new ArgumentOutOfRangeException(nameof(radius), radius, "반경은 0보다 커야 합니다.");

            var push = Vector2.zero;
            foreach(var neighbor in neighbors)
            {
                var away = self - neighbor;
                var distance = away.magnitude;
                if (distance >= radius || distance < MinDistance) continue;

                var strength = 1f - distance / radius;
                push += away / distance * strength;
            }
            return push;
        }
    }
}

