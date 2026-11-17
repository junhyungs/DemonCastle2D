using System;

namespace DemonCastle.Core
{
    public abstract class StateBase<TStateId> : IState where TStateId : struct, Enum
    {
        protected StateMachine<TStateId> Machine { get; }

        protected StateBase(StateMachine<TStateId> machine)
        {            
            Machine = machine ?? throw new ArgumentNullException(nameof(machine));
        }

        public virtual void Enter() { }
        public virtual void FixedTick(float fixedDeltaTime) { }
        public abstract void Tick(float deltaTime);
        public virtual void Exit() { }
    }
}

