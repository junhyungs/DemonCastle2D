using DemonCastle.Combat;
using NUnit.Framework;
using System;
using Unity.Android.Gradle;

namespace DemonCastle.Tests
{
    public class InvincibilityTests
    {
        private class FakeDamageable : IDamageable
        {
            public int HitCount { get; private set; }
            public void TakeDamage(DamageInfo damage)
            {
                HitCount++;
            }
        }

        [Test]
        public void 처음_맞으면_전달된다()
        {
            var fakeDamageable = new FakeDamageable();
            var invincibility = new Invincibility(fakeDamageable, 1f);
            invincibility.TakeDamage(new DamageInfo(1));

            Assert.AreEqual(1, fakeDamageable.HitCount);
        }

        [Test]
        public void 맞은_직후_다시_맞으면_막힌다()
        {
            var fakeDamageable = new FakeDamageable();
            var invincibility = new Invincibility(fakeDamageable, 1f);
            invincibility.TakeDamage(new DamageInfo(1));
            invincibility.TakeDamage(new DamageInfo(1));

            Assert.AreEqual(1, fakeDamageable.HitCount);
        }

        [Test]
        public void 무적_시간이_지난_뒤_Tick_맞으면_다시_전달된다()
        {
            var fakeDamageable = new FakeDamageable();
            var invincibility = new Invincibility(fakeDamageable, 1f);
            invincibility.TakeDamage(new DamageInfo(1));

            Assert.AreEqual(1, fakeDamageable.HitCount);

            invincibility.Tick(1f);
            invincibility.TakeDamage(new DamageInfo(1));
            Assert.AreEqual(2, fakeDamageable.HitCount);
        }

        [Test]
        public void Block_중에는_무적_시간과_상관없이_막힌다()
        {
            var fakeDamageable = new FakeDamageable();
            var invincibility = new Invincibility(fakeDamageable, 1f);

            invincibility.Block();
            invincibility.TakeDamage(new DamageInfo(1));
            Assert.AreEqual(0, fakeDamageable.HitCount);
        }

        [Test]
        public void Block두_번_Unblock_한_번_아직_막힌다()
        {
            var fakeDamageable = new FakeDamageable();
            var invincibility = new Invincibility(fakeDamageable, 1f);

            invincibility.Block();
            invincibility.Block();

            invincibility.Unblock();
            invincibility.TakeDamage(new DamageInfo(1));

            Assert.AreEqual(0, fakeDamageable.HitCount);
        }

        [Test]
        public void Block_없이_Unblock_예외()
        {
            var fakeDamageable = new FakeDamageable();
            var invincibility = new Invincibility(fakeDamageable, 1f);

            Assert.Throws<InvalidOperationException>(() => invincibility.Unblock());
        }

        [Test]
        public void 생성자_검사_inner가_null()
        {
            Assert.Throws<ArgumentNullException>(() => new Invincibility(null, 1f)); 
        }

        [Test]
        public void 생성자_검사_무적시간이_음수()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Invincibility(new FakeDamageable(), -1f));
        }
    }
}

