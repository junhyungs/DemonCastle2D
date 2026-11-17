using System;
using UnityEngine;
using System.Collections.Generic;
using DemonCastle.Combat;

namespace DemonCastle.Weapons
{
    public sealed class MeleeWeapon : IWeapon
    {
        private readonly WeaponDefinition _definition;
        private readonly Transform _owner;
        private readonly ContactFilter2D _filter;
        private readonly List<Collider2D> _hits = new List<Collider2D>();
        private readonly HashSet<IDamageable> _damaged = new HashSet<IDamageable>();

        private float _remainingCooldown;

        public event Action<Vector2> Attacked;
        public bool IsReady => _remainingCooldown <= 0f;

        public MeleeWeapon(
            WeaponDefinition definition,
            Transform owner,
            LayerMask targetLayers
            )
        {
            _definition = definition != null ? definition : throw new ArgumentNullException(nameof(definition));
            _owner = owner != null ? owner : throw new ArgumentNullException( nameof(owner));

            _filter = new ContactFilter2D();
            _filter.SetLayerMask(targetLayers);
            _filter.useTriggers = true;
        }

        public bool TryAttack(Vector2 direction)
        {
            if (!IsReady || direction == Vector2.zero) return false;

            _remainingCooldown = _definition.Cooldown;
            HitTargets(direction);
            Attacked?.Invoke(direction);
            return true;
        }

        public void Tick(float deltaTime)
        {
            if (_remainingCooldown > 0f) _remainingCooldown -= deltaTime;
        }

        private void HitTargets(Vector2 direction)
        {
            Vector2 origin = _owner.position;
            Physics2D.OverlapCircle(origin, _definition.Range, _filter, _hits);

            _damaged.Clear();
            foreach(var hit in _hits)
            {
                //Collider.ClosestPoint()-> 콜라이더 위의 점 중에서 origin에 가장 가까운 점. 
                // 큰 적도 가장자리가 부채꼴에 걸리면 맞도록, 중심이 아니라 가장 가까운 점으로 잰다.
                var closest = hit.ClosestPoint(origin);
                if (!MeleeArc.Contains(origin, direction, closest, _definition.Range, _definition.ArcDegrees)) continue;

                if (!hit.TryGetComponent<IDamageable>(out var target)) continue;
                if (!_damaged.Add(target)) continue;

                target.TakeDamage(new DamageInfo(_definition.Damage));
            }
        }
    }
}

