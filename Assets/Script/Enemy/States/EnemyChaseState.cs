using DemonCastle.Core;

namespace DemonCastle.Enemy
{
    public sealed class EnemyChaseState : EnemyState
    {
        public EnemyChaseState(
            StateMachine<EnemyStateId> machine,
            EnemyContext context
            ) : base(machine, context) { }
        
        public override void Tick(float deltaTime)
        {
            var definition = Context.Definition;
            if (Context.DistanceToTarget <= definition.LoseRange) return;

            Machine.ChangeState(EnemyStateId.Idle);
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            var definition = Context.Definition;
            if (Context.DistanceToTarget <= definition.StopDistance)
            {
                StopMoving();
                return;
            }                   
            MoveToward(Context.TargetPosition, definition.ChaseSpeed);
        }

        public override void Exit()
        {
            StopMoving();
        }
    }
}

