using DemonCastle.Controls;
using DemonCastle.Core;
using System;
using UnityEngine;

namespace DemonCastle.Player
{
    public class PlayerMoveState : PlayerGroundedState
    {
        private readonly PlayerStats _stats;
        public PlayerMoveState(StateMachine<PlayerStateId> machine, GameControls.PlayerActions input, Rigidbody2D body,
            PlayerStats stats, RollAbility rollAbility) : base(machine, input, body, rollAbility)
        {
            _stats = stats?? throw new ArgumentNullException(nameof(stats));
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            var readValue = ReadMove();
            Body.linearVelocity = PlayerMovement.CalculateVelocity(readValue, _stats.MoveSpeed);            
        }

        public override void Tick(float deltaTime)
        {            
            if (TryStartRoll()) return;

            if (ReadMove() == Vector2.zero)
            {
                Machine.ChangeState(PlayerStateId.Idle);
                return;
            }
        }
    }
}

