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
        private readonly IWeaponMotion _motion;
        private readonly ContactFilter2D _filter;
        private readonly List<Collider2D> _hits = new List<Collider2D>();
        private readonly HashSet<IDamageable> _damaged = new HashSet<IDamageable>();

        private bool _isSwinging;
        private Vector2 _swingDirection;
        private float _swingTimeLeft;
        private float _cooldownLeft;

        public bool IsReady => !_isSwinging && _cooldownLeft <= 0;

        public MeleeWeapon(
            WeaponDefinition definition,
            Transform owner,
            IWeaponMotion motion,
            LayerMask targetLayers
            )
        {
            _definition = definition != null ? definition : throw new ArgumentNullException(nameof(definition));
            _owner = owner != null ? owner : throw new ArgumentNullException( nameof(owner));
            _motion = motion ?? throw new ArgumentNullException(nameof(motion));

            _filter = new ContactFilter2D();
            _filter.SetLayerMask(targetLayers);
            _filter.useTriggers = true;

            _motion.Hit += OnHit;
            _motion.Ended += OnEnded;
        }

        public bool TryAttack(Vector2 direction)
        {
            if (!IsReady || direction == Vector2.zero) return false;

            _isSwinging = true;
            _swingDirection = direction;
            _swingTimeLeft = _definition.SwingTimeout;
            _motion.Play();            
            return true;
        }

        public void Tick(float deltaTime)
        {
            if (_cooldownLeft > 0) _cooldownLeft -= deltaTime;
            if (!_isSwinging) return;

            _swingTimeLeft -= deltaTime;
            if (_swingTimeLeft > 0) return;

            // 정상이라면 클립의 OnSwingEnd가 먼저 온다. 여기까지 왔다면 클립에 이벤트가 빠졌거나 재생이 끊긴 것.
            Debug.LogWarning($"[{nameof(MeleeWeapon)}] 끝 이벤트 없이 안전 시간({_definition.SwingTimeout}초)이 지나 공격을 강제로 끝냅니다.");
            OnEnded();
        }

        private void OnHit()
        {
            if (!_isSwinging) return;
            HitTargets(_swingDirection);
        }

        private void OnEnded()
        {
            if (!_isSwinging) return;

            _isSwinging = false;
            _cooldownLeft = _definition.Cooldown;
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

