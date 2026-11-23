using DemonCastle.Core;
using System.Collections.Generic;
using System;
using DemonCastle.Combat;

namespace DemonCastle.Enemy
{
    public class EnemyBrain
    {
        private readonly StateMachine<EnemyStateId> _machine = new StateMachine<EnemyStateId>();
        private readonly List<ITickable> _tickables = new List<ITickable>();
        private readonly EnemyHurtState _hurt;

        public EnemyStateId CurrentState => _machine.CurrentId;
        public EnemyBrain(
            EnemyContext context,
            ITickable invincibility,
            EnemyStateId initialState
            )
        {
            if(context == null) throw new ArgumentNullException(nameof(context));
            if(invincibility == null) throw new ArgumentNullException(nameof(invincibility));
            _tickables.Add(invincibility);

            _hurt = new EnemyHurtState(_machine, context);

            _machine.RegisterAll(Create);
            _machine.ChangeState(initialState);

            IState Create(EnemyStateId id)
            {
                return id switch
                {
                    EnemyStateId.Idle => new EnemyIdleState(_machine, context),
                    EnemyStateId.Wander => new EnemyWanderState(_machine, context),
                    EnemyStateId.Chase => new EnemyChaseState(_machine, context),
                    EnemyStateId.Hurt => _hurt,
                    _ => throw new ArgumentOutOfRangeException(nameof(id), id, "이 상태를 만드는 코드가 없습니다.")
                };
            }
        }

        public void Tick(float deltaTime)
        {
            foreach(var tickable in _tickables) tickable.Tick(deltaTime);
            _machine.Tick(deltaTime);
        }

        public void FixedTick(float fixedDeltaTime)
        {
            _machine.FixedTick(fixedDeltaTime);
        }

        public void ReceiveHit(DamageInfo hit)
        {
            if (hit.Knockback <= 0f) return;

            _hurt.Prepare(hit);
            _machine.ChangeState(EnemyStateId.Hurt);
        }
    }
}

