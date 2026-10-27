using System;

namespace DemonCastle.Combat
{
    public delegate void HealthChangedHandler(int current, int max);
    public class Health : IDamageable, IHealable
    {
        public int Max { get; }
        public int Current { get; private set; }
        public bool IsDead => Current <= 0;

        public event HealthChangedHandler Changed;
        public event Action Died;

        public Health(int max)
        {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max), max, "최대 체력은 0보다 커야합니다.");
            Max = max;
            Current = max;
        }

        public void TakeDamage(DamageInfo damage)
        {
            if (damage.Amount <= 0) throw new ArgumentOutOfRangeException(nameof(damage.Amount), damage.Amount, "피해량은 0보다 작을 수 없습니다.");
            if (IsDead) return;

            Current = Math.Max(0, Current - damage.Amount);
            Changed?.Invoke(Current, Max);

            if (IsDead) Died?.Invoke();
        }

        public void Heal(int amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), amount, "회복량은 0보다 커야합니다.");
            if (IsDead) return;

            var previousCurrent = Current;
            Current = Math.Min(Max, Current + amount);

            var isChanged = previousCurrent != Current;
            if (isChanged) Changed?.Invoke(Current, Max);
        }
    }
}

