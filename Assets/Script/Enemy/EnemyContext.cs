using UnityEngine;
using DemonCastle.Navigation;
using System;

namespace DemonCastle.Enemy
{
    public class EnemyContext
    {
        public Rigidbody2D Body { get; }
        public Collider2D BodyCollider { get; }
        public EnemyDefinition Definition { get; }
        public Collider2D TargetCollider { get; }        
        public INavigator Navigator { get; }
        public Vector2 Home { get; }
        public NeighborSensor Neighbors { get; }

        public Vector2 Position => Body.position;
        public Vector2 TargetPosition => TargetCollider.transform.position;
        public float DistanceToTarget => BodyCollider.Distance(TargetCollider).distance;

        public EnemyContext(
            Rigidbody2D body,
            Collider2D bodyCollider,
            EnemyDefinition definition,
            Collider2D targetCollider,            
            INavigator navigator,
            Vector2 home,
            NeighborSensor neighborSensor
            )
        {
            Body = body != null ? body : throw new ArgumentNullException(nameof(body));
            BodyCollider = bodyCollider != null ? bodyCollider : throw new ArgumentNullException(nameof(bodyCollider));
            Definition = definition != null ? definition : throw new ArgumentNullException(nameof(definition));
            TargetCollider = targetCollider != null ? targetCollider : throw new ArgumentNullException(nameof(targetCollider));            
            Navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            Home = home;
            Neighbors = neighborSensor ?? throw new ArgumentNullException(nameof(neighborSensor));
        }

    }
}

