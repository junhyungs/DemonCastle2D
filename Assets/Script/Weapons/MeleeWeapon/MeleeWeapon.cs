using System;
using UnityEngine;
using System.Collections.Generic;
using DemonCastle.Combat;

namespace DemonCastle.Weapons
{
    public sealed class MeleeWeapon : IWeapon
    {
        private const int NotAttacking = -1;

        private readonly WeaponDefinition _definition;
        private readonly Transform _owner;
        private readonly IWeaponMotion _motion;
        private readonly ContactFilter2D _filter;
        private readonly List<Collider2D> _hits = new List<Collider2D>();
        private readonly HashSet<IDamageable> _damaged = new HashSet<IDamageable>();

        private int _stepIndex = NotAttacking;
        private Vector2 _attackDirection = Vector2.right;
        private bool _hasQueuedInput;
        private Vector2 _queuedDirection;
        private bool _isComboWindowOpen;
        private float _swingTimeLeft;
        private float _cooldownLeft;
        
        public bool IsAttacking => _stepIndex != NotAttacking;
        public bool IsReady => !IsAttacking && _cooldownLeft <= 0f;
        public Vector2 AttackDirection => _attackDirection;
        public float MoveMultiplier => IsAttacking ? CurrentStep.MoveMultiplier : 1f;

        private AttackStep CurrentStep => _definition.Steps[_stepIndex];
        private bool IsLastStep => _stepIndex == _definition.Steps.Count - 1;

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

            if(_definition.Steps.Count == 0) throw new ArgumentException("WeaponDefinition에 공격 단계(Steps)가 하나도 없습니다.", nameof(definition));

            _filter = new ContactFilter2D();
            _filter.SetLayerMask(targetLayers);
            _filter.useTriggers = true;

            _motion.Hit += OnHit;
            _motion.ComboWindowOpened += OnComboWindowOpened;
            _motion.Ended += OnEnded;
        }

        public bool TryAttack(Vector2 direction)
        {
            if (direction == Vector2.zero) return false;

            if (!IsAttacking)
            {
                if (_cooldownLeft > 0f) return false;
                StartStep(0, direction);
                return true;                
            }

            if (IsLastStep) return false;

            _hasQueuedInput = true;
            _queuedDirection = direction;
            if (_isComboWindowOpen) StartStep(_stepIndex + 1, _queuedDirection);
            return true;
        }

        public void Cancel()
        {
            if (!IsAttacking) return;
            _motion.Stop();
            EndCombo();
        }

        public void Tick(float deltaTime)
        {
            if (_cooldownLeft > 0) _cooldownLeft -= deltaTime;
            if (!IsAttacking) return;

            _swingTimeLeft -= deltaTime;
            if (_swingTimeLeft > 0) return;

            // 정상이라면 클립의 OnSwingEnd가 먼저 온다. 여기까지 왔다면 클립에 이벤트가 빠졌거나 재생이 끊긴 것.
            Debug.LogWarning($"[{nameof(MeleeWeapon)}] 끝 이벤트 없이 안전 시간({_definition.SwingTimeout}초)이 지나 공격을 강제로 끝냅니다. {_stepIndex + 1} 타");
            _motion.Stop();
            EndCombo();
        }

        private void OnHit()
        {
            if (!IsAttacking) return;
            HitTargets(CurrentStep);
        }

        private void OnComboWindowOpened()
        {
            if (!IsAttacking) return;
            _isComboWindowOpen = true;
            if (_hasQueuedInput) StartStep(_stepIndex + 1, _queuedDirection);
        }

        private void OnEnded()
        {
            if (!IsAttacking) return;
            EndCombo();
        }

        private void StartStep(int index, Vector2 direction)
        {
            _stepIndex = index;
            _attackDirection = direction;
            _hasQueuedInput = false;
            _isComboWindowOpen = false;
            _swingTimeLeft = _definition.SwingTimeout;
            _motion.Play(index);
        }

        private void EndCombo()
        {
            _stepIndex = NotAttacking;
            _hasQueuedInput = false;
            _isComboWindowOpen = false;
            _cooldownLeft = _definition.Cooldown;
        }

        private void HitTargets(AttackStep step)
        {
            Vector2 origin = _owner.position;
            Physics2D.OverlapCircle(origin, step.Range, _filter, _hits);

            _damaged.Clear();
            foreach(var hit in _hits)
            {
                //Collider.ClosestPoint()-> 콜라이더 위의 점 중에서 origin에 가장 가까운 점. 
                // 큰 적도 가장자리가 부채꼴에 걸리면 맞도록, 중심이 아니라 가장 가까운 점으로 잰다.
                var closest = hit.ClosestPoint(origin);
                if (!MeleeArc.Contains(origin, _attackDirection, closest, step.Range, step.ArcDegrees)) continue;

                if (!hit.TryGetComponent<IDamageable>(out var target)) continue;
                if (!_damaged.Add(target)) continue;

                target.TakeDamage(new DamageInfo(step.Damage));
            }
        }
    }
}

