using UnityEngine;
using System;

namespace DemonCastle.Weapons
{
    [Serializable]
    public sealed class AttackStep
    {
        [SerializeField, Min(1)] private int _damage = 1;
        [SerializeField, Min(0.1f)] private float _range = 1.2f;
        [SerializeField, Range(10f, 360f)] private float _arcDegrees = 200f;
        [Tooltip("공격 중 이동속도 배율. 0이면 제자리, 1이면 평소 속도")]
        [SerializeField, Range(0f, 1f)] private float _moveMultiplier = 0.5f;

        public int Damage => _damage;
        public float Range => _range;
        public float ArcDegrees => _arcDegrees;
        public float MoveMultiplier => _moveMultiplier;

        public AttackStep() { }

        public AttackStep(int damage, float range, float acrDegrees, float moveMultiplier)
        {
            _damage = damage;
            _range = range;
            _arcDegrees = acrDegrees;
            _moveMultiplier = moveMultiplier;
        }
    }
}


