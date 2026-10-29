using DemonCastle.Combat;
using DemonCastle.Controls;
using DemonCastle.Core;
using System;
using UnityEngine;

namespace DemonCastle.Player
{
    public class PlayerRollState : PlayerState
    {
        private readonly IAimInput _aim;        
        private readonly PlayerStats _stats;
        private readonly RollAbility _rollAbility;
        private readonly Invincibility _invincibility;

        private Vector2 _direction;
        private float _speed;
        private float _remainingTime;        

        public PlayerRollState(
            StateMachine<PlayerStateId> machine,
            GameControls.PlayerActions input,
            Rigidbody2D body,
            IAimInput aim,
            PlayerStats stats,
            RollAbility rollAbility,
            Invincibility invincibility
            ) : base(machine, input, body)
        {
            _aim = aim?? throw new ArgumentNullException(nameof(aim));
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));
            _rollAbility = rollAbility ?? throw new ArgumentNullException(nameof(rollAbility));
            _invincibility = invincibility ?? throw new ArgumentNullException(nameof(invincibility));
        }

        public override void Enter()
        {
            Vector2 moveInput = ReadMove();
            _direction = moveInput != Vector2.zero ? moveInput.normalized : _aim.Direction;
            _speed = _stats.RollSpeed;
            _remainingTime = _stats.RollDuration;
            Body.linearVelocity = _direction * _speed;

            _invincibility.Block();
        }

        // 구르기는 물리 이동이라 FixedTick에서 시간을 센다.
        public override void Tick(float deltaTime) { }
        
        public override void FixedTick(float fixedDeltaTime)
        {
            Body.linearVelocity = _direction * _speed;

            _remainingTime -= fixedDeltaTime;
            if (_remainingTime > 0f) return;

            var next = ReadMove() != Vector2.zero ? PlayerStateId.Move : PlayerStateId.Idle;
            Machine.ChangeState(next);
        }

        public override void Exit()
        {
            Body.linearVelocity = Vector2.zero;
            _rollAbility.StartCooldown();
            _invincibility.Unblock();
        }
    }
}

