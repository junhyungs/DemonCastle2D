using UnityEngine;
using System;
using DemonCastle.Controls;
using DemonCastle.Combat;

namespace DemonCastle.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _aimIndicatorDistance = 0.8f;        
        [SerializeField] private Transform _aimIndicator;
        [SerializeField] private Hurtbox _hurtbox;
        [SerializeField] private PlayerStateId _currentStateForDebug;

        //TODO 개발용 테스트 코드 이후 삭제필요.
        [ContextMenu("디버그/피해 1")]
        private void DebugTakeDamage() => Health?.TakeDamage(new DamageInfo(1));
        [ContextMenu("디버그/회복 1")]
        private void DebugHeal() => Health?.Heal(1);

        private Rigidbody2D _rigidbody;
        private PlayerStats _stats;
        private PlayerBrain _brain;
        private PlayerDefinition _definition;
        private IAimInput _aim;

        public Health Health { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            enabled = false;
        }

        private void FixedUpdate()
        {
            _brain.FixedTick(Time.fixedDeltaTime);
        }

        private void Update()
        {
            _aim.Tick(transform.position);
            _aimIndicator.localPosition = _aim.Direction * _aimIndicatorDistance;

            _brain.Tick(Time.deltaTime);
            _currentStateForDebug = _brain.CurrentState;
        }

        private void OnDestroy()
        {
            if (_definition != null) _definition.Changed -= ApplyDefinition;
        }

        public void Initialize(GameControls.PlayerActions input, IAimInput aim, PlayerDefinition playerDefinition)
        {
            if (_aimIndicator == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_aimIndicator)}");
            if (_hurtbox == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_hurtbox)}");
            if (playerDefinition == null) throw new ArgumentNullException(nameof(playerDefinition));

            _aim = aim ?? throw new ArgumentNullException(nameof(aim));
            _definition = playerDefinition;

            _stats = new PlayerStats(
                playerDefinition.MoveSpeed,
                playerDefinition.RollSpeed,
                playerDefinition.RollDuration,
                playerDefinition.RollCooldown
            );

            Health = new Health(playerDefinition.MaxHealth);

            var invincibility = new Invincibility(Health, playerDefinition.HitInvincibleDuration);
            _hurtbox.Initialize(invincibility);
            _brain = new PlayerBrain(input, _rigidbody, _aim, _stats, invincibility);

            _definition.Changed += ApplyDefinition;
            enabled = true;
        }

        private void ApplyDefinition()
        {           
            _stats.MoveSpeed = _definition.MoveSpeed;
            _stats.RollSpeed = _definition.RollSpeed;
            _stats.RollDuration = _definition.RollDuration;
            _stats.RollCooldown = _definition.RollCooldown;
        }
    }

}
