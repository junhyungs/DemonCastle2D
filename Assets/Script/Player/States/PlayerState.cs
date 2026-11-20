using DemonCastle.Controls;
using DemonCastle.Core;
using UnityEngine;
using System;

namespace DemonCastle.Player
{
    public abstract class PlayerState : StateBase<PlayerStateId>
    {
        protected PlayerContext Context { get; }
        protected GameControls.PlayerActions Input => Context.Input;
        protected Rigidbody2D Body => Context.Body;        

        protected PlayerState(
            StateMachine<PlayerStateId> machine,
            PlayerContext context
            ) : base(machine)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }        

        protected Vector2 ReadMove() => Input.Move.ReadValue<Vector2>();        
        protected void ReturnToGround()
        {
            var next = ReadMove() != Vector2.zero ? PlayerStateId.Move : PlayerStateId.Idle;
            Machine.ChangeState(next);
        }
    }
}

