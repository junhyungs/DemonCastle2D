using DemonCastle.Combat;
using DemonCastle.Core;
using UnityEngine;

namespace DemonCastle.Enemy
{
    public sealed class EnemyHurtState : EnemyState
    {
        private DamageInfo _hit;
        private Vector2 _velocity;
        private float _deceleration;
        private float _remainingTime;

        public EnemyHurtState(
            StateMachine<EnemyStateId> machine,
            EnemyContext context
            ) : base(machine, context) { }

        public void Prepare(DamageInfo hit) => _hit = hit;

        public override void Enter()
        {
            var duration = Context.Definition.HurtDuration;
            _remainingTime = duration;
            _velocity = _hit.Direction * _hit.Knockback;
            _deceleration = _velocity.magnitude / duration;
            Context.Body.linearVelocity = _velocity;
        }

        public override void Tick(float deltaTime) { }

        public override void FixedTick(float fixedDeltaTime)
        {
            _velocity = Vector2.MoveTowards(_velocity, Vector2.zero, _deceleration * fixedDeltaTime);
            Context.Body.linearVelocity = _velocity;

            _remainingTime -= fixedDeltaTime;
            if (_remainingTime > 0f) return;

            Machine.ChangeState(EnemyStateId.Chase);
        }

        public override void Exit() => StopMoving();
    }
}

