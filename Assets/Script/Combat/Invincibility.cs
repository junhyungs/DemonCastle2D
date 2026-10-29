using DemonCastle.Core;
using System;

namespace DemonCastle.Combat
{
    public class Invincibility : IDamageable, ITickable
    {
        private readonly IDamageable _inner;
        private readonly float _durationAfterHit;
        private float _remainingTime;
        private int _blockCount;

        public bool IsActive => _remainingTime > 0f || _blockCount > 0;

        public Invincibility(IDamageable inner, float durationAfterHit)
        {
            if(durationAfterHit < 0f) throw new ArgumentOutOfRangeException(nameof(durationAfterHit), durationAfterHit, "무적 시간은 0 이상이어야 합니다.");
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _durationAfterHit = durationAfterHit;
        }

        public void TakeDamage(DamageInfo damage)
        {
            if (IsActive) return;

            _inner.TakeDamage(damage);
            _remainingTime = _durationAfterHit;
        }

        public void Block() => _blockCount++;

        public void Unblock()
        {
            if (_blockCount == 0) throw new InvalidOperationException("Block 없이 Unblock이 호출되었습니다.");
            _blockCount--;
        }

        public void Tick(float deltaTime)
        {
            if (_remainingTime > 0f) _remainingTime -= deltaTime;
        }
    }
}

