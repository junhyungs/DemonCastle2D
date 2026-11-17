using DemonCastle.Core;

namespace DemonCastle.Enemy
{
    public abstract class EnemyUnawareState : EnemyState
    {
        protected EnemyUnawareState(
            StateMachine<EnemyStateId> machine,
            EnemyContext context
            ) : base(machine, context) { }

        protected bool TryStartChase()
        {
            var definition = Context.Definition;
            if (Context.DistanceToTarget > definition.DetectRange) return false;

            Machine.ChangeState(EnemyStateId.Chase);
            return true;
        }
    }
}

