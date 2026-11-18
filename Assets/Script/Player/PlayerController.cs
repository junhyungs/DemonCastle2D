using UnityEngine;
using System;
using DemonCastle.Controls;
using DemonCastle.Combat;
using DemonCastle.Weapons;

namespace DemonCastle.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _aimIndicatorDistance = 0.8f;        
        [SerializeField] private Transform _aimIndicator;
        [SerializeField] private FacingView _facingView;
        [SerializeField] private Hurtbox _hurtbox;
        [SerializeField] private Collider2D _bodyCollider;        
        [SerializeField] private LayerMask _attackTargetLayers;
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
        private Facing _facing;

        public Health Health { get; private set; }
        public Collider2D BodyCollider => _bodyCollider;

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
            _facing.Follow(_aim.Direction);
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
            if (_bodyCollider == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_bodyCollider)}");
            if (_facingView == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_facingView)}");
            if (_attackTargetLayers.value == 0) throw new InvalidOperationException($"{nameof(_attackTargetLayers)}에 레이어가 하나도 없습니다.");
            if (playerDefinition == null) throw new ArgumentNullException(nameof(playerDefinition));
            if (playerDefinition.StartingWeapon == null) throw new InvalidOperationException($"PlayerDefinition에 시작 무기가 없습니다.");

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

            _facing = new Facing();
            var rollAbility = new RollAbility(_stats);
            var weapon = new MeleeWeapon(playerDefinition.StartingWeapon, transform, _attackTargetLayers);
            var context = new PlayerContext(input, _rigidbody, _aim, _facing, _stats, rollAbility, weapon, invincibility);
            _brain = new PlayerBrain(context);

            _facingView.Initialize(_facing);
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
