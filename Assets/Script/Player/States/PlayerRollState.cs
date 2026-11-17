using DemonCastle.Core;
using UnityEngine;

namespace DemonCastle.Player
{
    public sealed class PlayerRollState : PlayerState
    {
        private Vector2 _direction;
        private float _speed;
        private float _remainingTime;        

        public PlayerRollState(
            StateMachine<PlayerStateId> machine,
            PlayerContext context
            ) : base(machine, context) { }        

        public override void Enter()
        {
            Vector2 moveInput = ReadMove();
            _direction = moveInput != Vector2.zero ? moveInput.normalized : Context.Aim.Direction;
            _speed = Context.Stats.RollSpeed;
            _remainingTime = Context.Stats.RollDuration;
            Body.linearVelocity = _direction * _speed;

            Context.Invincibility.Block();
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
            Context.Roll.StartCooldown();
            Context.Invincibility.Unblock();
        }
    }
}

