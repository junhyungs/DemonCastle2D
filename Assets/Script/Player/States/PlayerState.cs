using DemonCastle.Controls;
using DemonCastle.Core;
using UnityEngine;
using System;

namespace DemonCastle.Player
{
    public abstract class PlayerState : IState
    {
        protected StateMachine<PlayerStateId> Machine { get; }
        protected GameControls.PlayerActions Input { get; }
        protected Rigidbody2D Body { get; }        

        protected PlayerState(StateMachine<PlayerStateId> machine, GameControls.PlayerActions input, Rigidbody2D body)
        {
            Machine = machine ?? throw new ArgumentNullException(nameof(machine));
            Input = input;
            Body = body != null ? body : throw new ArgumentNullException(nameof(body));            
        }        

        public virtual void Enter() { }        
        public virtual void FixedTick(float fixedDeltaTime) { }
        public abstract void Tick(float deltaTime);
        public virtual void Exit() { }

        protected Vector2 ReadMove() => Input.Move.ReadValue<Vector2>();        
    }
}

