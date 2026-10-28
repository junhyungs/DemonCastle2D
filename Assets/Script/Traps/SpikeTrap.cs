using UnityEngine;
using DemonCastle.Combat;

namespace DemonCastle.Traps
{
    [RequireComponent(typeof(Collider2D))]
    public class SpikeTrap : MonoBehaviour
    {
        [SerializeField, Min(1)] private int _damage = 1;

        private void Reset()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<IDamageable>(out var target)) return;
            target.TakeDamage(new DamageInfo(_damage));
        }
    }
}

