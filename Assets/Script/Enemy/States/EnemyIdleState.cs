using DemonCastle.Core;

namespace DemonCastle.Enemy
{
    public sealed class EnemyIdleState : EnemyUnawareState
    {
        private float _remainingTime;

        public EnemyIdleState(
            StateMachine<EnemyStateId> machine,
            EnemyContext context
            ) : base(machine, context) { }

        public override void Enter()
        {            
            var definition = Context.Definition;
            _remainingTime = UnityEngine.Random.Range(definition.IdleTimeMin, definition.IdleTimeMax);
        }

        public override void FixedTick(float fixedDeltaTime) => StopMoving();

        public override void Tick(float deltaTime)
        {
            if (TryStartChase()) return;

            _remainingTime -= deltaTime;
            if (_remainingTime > 0f) return;

            Machine.ChangeState(EnemyStateId.Wander);
        }
    }
}

