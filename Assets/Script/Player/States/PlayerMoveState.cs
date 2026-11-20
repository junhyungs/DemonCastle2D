using DemonCastle.Core;
using UnityEngine;

namespace DemonCastle.Player
{
    public sealed class PlayerMoveState : PlayerGroundedState
    {        
        public PlayerMoveState(
            StateMachine<PlayerStateId> machine,
            PlayerContext context
            ) : base(machine, context) { }
        
        public override void FixedTick(float fixedDeltaTime)
        {
            var readValue = ReadMove();
            var stats = Context.Stats;
            Body.linearVelocity = PlayerMovement.CalculateVelocity(readValue, stats.MoveSpeed);            
        }

        public override void Tick(float deltaTime)
        {            
            if (TryStartRoll()) return;
            if (TryStartAttack()) return;

            if (ReadMove() == Vector2.zero) Machine.ChangeState(PlayerStateId.Idle);
        }
    }
}

