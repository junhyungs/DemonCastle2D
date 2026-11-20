using DemonCastle.Core;
using UnityEngine;

namespace DemonCastle.Player
{
    public abstract class PlayerGroundedState : PlayerState
    {        
        protected PlayerGroundedState(
            StateMachine<PlayerStateId> machine,
            PlayerContext context
            ) : base(machine, context) { }
        
        protected bool TryStartRoll()
        {
            if (!Input.Roll.WasPressedThisFrame() || !Context.Roll.IsReady) return false;
            Machine.ChangeState(PlayerStateId.Roll);
            return true;
        }

        protected bool TryStartAttack()
        {
            if (!WasAttackPressed()) return false;
            if(!Context.Weapon.TryAttack(AttackDirection())) return false;

            Machine.ChangeState(PlayerStateId.Attack);
            return true;
        }        

        protected bool WasAttackPressed() => Input.Attack.WasPressedThisFrame() 
            || Input.AimKey.WasPressedThisFrame();

        protected Vector2 AttackDirection() => Context.Facing.ForwardFor(Context.Aim.Direction);
    }
}

