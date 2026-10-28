using UnityEngine;
using System;

namespace DemonCastle.Combat
{
    public class Hurtbox : MonoBehaviour, IDamageable
    {
        private IDamageable _target;

        public void Initialize(IDamageable target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }
        
        public void TakeDamage(DamageInfo damage)
        {
            if(_target == null) throw new InvalidOperationException($"'{name}'의 Hurtbox가 초기화되지 않았습니다.");
            _target.TakeDamage(damage);
        }
    }
}


