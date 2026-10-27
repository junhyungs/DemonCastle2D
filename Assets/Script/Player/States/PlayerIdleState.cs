using DemonCastle.Controls;
using DemonCastle.Core;
using UnityEngine;

namespace DemonCastle.Player
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(StateMachine<PlayerStateId> machine, GameControls.PlayerActions input,
            Rigidbody2D body, RollAbility rollAbility) : base(machine, input, body, rollAbility) { }

        public override void Enter()
        {
            Body.linearVelocity = Vector2.zero;
        }

        public override void Tick(float deltaTime)
        {
            if (TryStartRoll()) return;
            
            if(ReadMove() != Vector2.zero)
            {
                Machine.ChangeState(PlayerStateId.Move);
                return;
            }
        }
    }

}
