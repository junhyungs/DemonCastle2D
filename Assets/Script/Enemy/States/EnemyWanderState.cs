using DemonCastle.Core;
using UnityEngine;

namespace DemonCastle.Enemy
{
    public sealed class EnemyWanderState : EnemyUnawareState
    {
        private const float ArriveDistance = 0.1f;
        private const float GiveUpTime = 3f;

        private Vector2 _destination;
        private float _elapsedTime;

        public EnemyWanderState(
            StateMachine<EnemyStateId> machine,
            EnemyContext context
            ) : base(machine, context) { }

        public override void Enter()
        {
            var definition = Context.Definition;
            _destination = Context.Home + Random.insideUnitCircle * definition.WanderRadius;
            _elapsedTime = 0f;
        }

        public override void Tick(float deltaTime)
        {
            if (TryStartChase()) return;

            _elapsedTime += deltaTime;

            bool arrived = Vector2.Distance(Context.Position, _destination) <= ArriveDistance;
            if (arrived || _elapsedTime >= GiveUpTime) Machine.ChangeState(EnemyStateId.Idle);
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            var definition = Context.Definition;
            MoveToward(_destination, definition.WanderSpeed);
        }

        public override void Exit()
        {
            StopMoving();
        }
    }
}

