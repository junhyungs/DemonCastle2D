using UnityEngine;
using System;

namespace DemonCastle.Combat
{
    [RequireComponent(typeof(Collider2D))]
    public class ContactHitbox : MonoBehaviour
    {
        private int _damage;

        private void Reset()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        public void Initialize(int damage)
        {
            if (damage <= 0) throw new ArgumentOutOfRangeException(nameof(damage), damage, "피해량은 0보다 커야합니다.");
            _damage = damage;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (_damage == 0) throw new InvalidOperationException($"'{name}'의 ContactHitbox가 초기화되지 않았습니다.");
            if (!other.TryGetComponent<IDamageable>(out var target)) return;
            target.TakeDamage(new DamageInfo(_damage));
        }
    }
}


