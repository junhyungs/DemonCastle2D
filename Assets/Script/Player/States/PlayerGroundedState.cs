using DemonCastle.Core;

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

        protected void TryAttack()
        {
            if (!WasAttackPressed()) return;
            Context.Weapon.TryAttack(Context.Facing.Forward);
        }

        private bool WasAttackPressed() => Input.Attack.WasPressedThisFrame() 
            || Input.AimKey.WasPressedThisFrame();        
    }
}

