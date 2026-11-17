using DemonCastle.Core;
using DemonCastle.Navigation;
using System;

namespace DemonCastle.Enemy
{
    public abstract class EnemyState : StateBase<EnemyStateId>
    {
        protected EnemyContext Context { get; }

        protected EnemyState(
            StateMachine<EnemyStateId> machine,
            EnemyContext context
            ) : base(machine)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected void MoveToward(UnityEngine.Vector2 destination, float speed)
        {
            var definition = Context.Definition;
            var navigator = Context.Navigator;
            var neighbors = Context.Neighbors;

            var toDestination = navigator.GetDirection(Context.Position, destination);
            var separation = Separation.Calculate(
                    Context.Position,
                    neighbors.Scan(),
                    definition.SeparationRadius
                );
            var direction = toDestination + separation * definition.SeparationWeight;

            var body = Context.Body;
            body.linearVelocity = UnityEngine.Vector2.ClampMagnitude(direction, 1f) * speed;
        }

        protected void StopMoving()
        {
            var body = Context.Body;
            body.linearVelocity = UnityEngine.Vector2.zero;
        }
    }
}


