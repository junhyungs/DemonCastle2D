using DemonCastle.Core;
using UnityEngine;

namespace DemonCastle.Player
{
    public sealed class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(
            StateMachine<PlayerStateId> machine,
            PlayerContext context
            ) : base(machine, context) { }
        
        public override void FixedTick(float fixedDeltaTime)
        {
            Body.linearVelocity = Vector2.zero;
        }

        public override void Tick(float deltaTime)
        {
            if (TryStartRoll()) return;

            TryAttack();
            
            if(ReadMove() != Vector2.zero) Machine.ChangeState(PlayerStateId.Move);
        }
    }

}
