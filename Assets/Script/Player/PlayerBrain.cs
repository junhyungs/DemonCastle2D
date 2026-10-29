using DemonCastle.Controls;
using DemonCastle.Core;
using UnityEngine;
using System;
using System.Collections.Generic;
using DemonCastle.Combat;

namespace DemonCastle.Player
{
    public class PlayerBrain
    {
        private readonly StateMachine<PlayerStateId> _machine = new StateMachine<PlayerStateId>();        
        private readonly List<ITickable> _tickables = new List<ITickable>();
        public PlayerStateId CurrentState => _machine.CurrentId;

        public PlayerBrain(GameControls.PlayerActions input, 
            Rigidbody2D body, IAimInput aim, PlayerStats stats, Invincibility invincibility)
        {
            var rollAbility = new RollAbility(stats);
            _tickables.Add(rollAbility);
            _tickables.Add(invincibility);

            _machine.RegisterAll(Create);
            _machine.ChangeState(PlayerStateId.Idle);
            
            IState Create(PlayerStateId id)
            {
                return id switch
                {
                    PlayerStateId.Idle => new PlayerIdleState(_machine, input, body, rollAbility),
                    PlayerStateId.Move => new PlayerMoveState(_machine, input, body, stats, rollAbility),
                    PlayerStateId.Roll => new PlayerRollState(_machine, input, body, aim, stats, rollAbility, invincibility),
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

