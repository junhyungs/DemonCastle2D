using UnityEngine;

namespace DemonCastle.Combat
{
    public class Hurtbox : MonoBehaviour, IDamageable
    {
        [SerializeField, Min(1)] private int _health;
        private Health _hurtboxHealth;

        private void Awake()
        {
            _hurtboxHealth = new Health(_health);
        }

        public void TakeDamage(DamageInfo damage)
        {
           _hurtboxHealth.TakeDamage(damage);
        }
    }
}


