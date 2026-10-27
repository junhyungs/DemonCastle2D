using DemonCastle.Controls;
using DemonCastle.Core;
using UnityEngine;
using System;

namespace DemonCastle.Player
{
    public abstract class PlayerGroundedState : PlayerState
    {
        private readonly RollAbility _rollAbility;
        protected PlayerGroundedState(StateMachine<PlayerStateId> machine, GameControls.PlayerActions input,
            Rigidbody2D body, RollAbility rollAbility) : base(machine, input, body)
        {
            _rollAbility = rollAbility ?? throw new ArgumentNullException(nameof(rollAbility));
        }

        protected bool TryStartRoll()
        {
            if (!Input.Roll.WasPressedThisFrame() || !_rollAbility.IsReady) return false;
            Machine.ChangeState(PlayerStateId.Roll);
            return true;
        }
    }
}

