using DemonCastle.Core;
using System;
using System.Collections.Generic;

namespace DemonCastle.Player
{
    public class PlayerBrain
    {
        private readonly StateMachine<PlayerStateId> _machine = new StateMachine<PlayerStateId>();        
        private readonly List<ITickable> _tickables = new List<ITickable>();
        public PlayerStateId CurrentState => _machine.CurrentId;

        public PlayerBrain(PlayerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            
            _tickables.Add(context.Roll);
            _tickables.Add(context.Invincibility);
            _tickables.Add(context.Weapon);

            _machine.RegisterAll(Create);
            _machine.ChangeState(PlayerStateId.Idle);
            
            IState Create(PlayerStateId id)
            {
                return id switch
                {
                    PlayerStateId.Idle => new PlayerIdleState(_machine, context),
                    PlayerStateId.Move => new PlayerMoveState(_machine, context),
                    PlayerStateId.Roll => new PlayerRollState(_machine, context),
                    PlayerStateId.Attack => new PlayerAttackState(_machine, context),
                    _ => throw new ArgumentOutOfRangeException(nameof(id), id, "이 상태를 만드는 코드가 없습니다.")
                };
            }
        }

        public void Tick(float deltaTime)
        {
            foreach(var tickable in _tickables) tickable.Tick(deltaTime);
            _machine.Tick(deltaTime);
        }

        public void FixedTick(float fixedDeltaTime) => _machine.FixedTick(fixedDeltaTime);
    }
}

