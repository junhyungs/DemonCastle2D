using UnityEngine;
using DemonCastle.Combat;
using System.Collections.Generic;

namespace DemonCastle.Traps
{
    [RequireComponent(typeof(Collider2D))]
    public class SpikeTrap : MonoBehaviour
    {
        [SerializeField, Min(1)] private int _damage = 1;
        [Tooltip("같은 대상을 다시 찌르기까지의 간격(초)")]
        [SerializeField, Min(0.05f)] private float _hitInterval = 0.5f;

        private readonly Dictionary<IDamageable, float> _nextHitTime = new Dictionary<IDamageable, float>();

        private void Reset()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.TryGetComponent<IDamageable>(out var target)) return;
            if (_nextHitTime.TryGetValue(target, out var next) && Time.time < next) return;

            _nextHitTime[target] = Time.time + _hitInterval;
            target.TakeDamage(new DamageInfo(_damage));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<IDamageable>(out var target)) _nextHitTime.Remove(target);
        }
    }
}

