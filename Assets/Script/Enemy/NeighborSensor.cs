using System;
using System.Collections.Generic;
using UnityEngine;

namespace DemonCastle.Enemy
{
    public sealed class NeighborSensor
    {
        private readonly Collider2D _self;
        private float _radius;
        private readonly ContactFilter2D _filter;
        private readonly List<Collider2D> _hits = new List<Collider2D>();
        private readonly List<Vector2> _positions = new List<Vector2>();

        public NeighborSensor(Collider2D self, LayerMask neighborLayers, float radius)
        {
            _self = self != null ? self : throw new ArgumentNullException(nameof(self));
            if (radius <= 0f) throw new ArgumentOutOfRangeException(nameof(radius), radius, "반경은 0보다 커야 합니다.");

            _radius = radius;
            _filter = new ContactFilter2D();
            _filter.SetLayerMask(neighborLayers);
            _filter.useTriggers = false;
        }

        public IReadOnlyList<Vector2> Scan()
        {
            _positions.Clear();
            Physics2D.OverlapCircle(_self.transform.position, _radius, _filter, _hits);

            foreach(var hit in _hits)
            {
                if (hit == _self) continue;
                _positions.Add(hit.transform.position);
            }
            return _positions;
        }
    }
}

