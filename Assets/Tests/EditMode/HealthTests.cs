using DemonCastle.Combat;
using NUnit.Framework;
using System;

namespace DemonCastle.Tests
{
    public class HealthTests
    {
        [Test]
        public void 생성자_처음엔_최대체력()
        {
            var playerHealth = new Health(10);
            Assert.IsTrue(playerHealth.Current == playerHealth.Max);
            Assert.IsFalse(playerHealth.IsDead);
        }

        [Test]
        public void TakeDamage_체력이줄고_Changed에현재값과최대값()
        {
            var playerHealth = new Health(5);
            var currentValue = 0;
            var maxValue = 0;

            playerHealth.Changed += (current, max) =>
            {
                currentValue = current;
                maxValue = max;
            };

            playerHealth.TakeDamage(new DamageInfo(2));
            Assert.AreEqual(3, currentValue);
            Assert.AreEqual(5, maxValue);
        }

        [Test]
        public void TakeDamage_체력이0이되면_Died가한번울림()
        {
            var playerHealth = new Health(5);
            var diedCount = 0;
            playerHealth.Died += () => diedCount++;

            playerHealth.TakeDamage(new DamageInfo(5));
            Assert.AreEqual(1, diedCount);
        }

        [Test]
        public void TakeDamage_죽은뒤또맞아도_아무일없음()
        {
            var playerHealth = new Health(5);
            var diedCount = 0;
            var changedCount = 0;

            playerHealth.Changed += (current, max) => changedCount++;
            playerHealth.Died += () => diedCount++;

            playerHealth.TakeDamage(new DamageInfo(5));
            playerHealth.TakeDamage(new DamageInfo(5));

            Assert.AreEqual(1, diedCount);
            Assert.AreEqual(1, changedCount);
        }

        [Test]
        public void TakeDamage_체력보다큰피해_0에서멈춤()
        {
            var playerHealth = new Health(3);
            playerHealth.TakeDamage(new DamageInfo(10));

            Assert.AreEqual(0, playerHealth.Current);
        }

        [Test]
        public void TakeDamage_피해가0이하면_예외()
        {
            var playerHealth = new Health(3);
            Assert.Throws<ArgumentOutOfRangeException>(() => playerHealth.TakeDamage(new DamageInfo(0)));
        }

        [Test]
        public void Heal_최대체력을넘지않음()
        {
            var playerHealth = new Health(3);
            var healAmount = 10;

            playerHealth.TakeDamage(new DamageInfo(2));
            playerHealth.Heal(healAmount);

            Assert.AreEqual(3, playerHealth.Current);
        }

        [Test]
        public void Heal_가득찬상태면_Changed가안울림()
        {
            var playerHealth = new Health(3);
            var healAmount = 10;
            var changedCount = 0;

            playerHealth.Changed += (current, max) => changedCount++;
            playerHealth.Heal(healAmount);

            Assert.AreEqual(0, changedCount);
        }

        [Test]
        public void Heal_체력이바뀌면_Changed가울림()
        {
            var playerHealth = new Health(3);
            var healAmount = 10;
            var changedCount = 0;
            
            playerHealth.TakeDamage(new DamageInfo(2));

            playerHealth.Changed += (current, max) => changedCount++;
            playerHealth.Heal(healAmount);

            Assert.AreEqual(1, changedCount);
        }

        [Test]
        public void Heal_죽은상태면_아무것도안함()
        {
            var playerHealth = new Health(3);
            var healAmount = 10;
            var changedCount = 0;

            playerHealth.Changed += (current, max) => changedCount++;

            playerHealth.TakeDamage(new DamageInfo(3));
            playerHealth.Heal(healAmount);
            playerHealth.Heal(healAmount);

            Assert.AreEqual(1, changedCount);
            Assert.AreEqual(0, playerHealth.Current);
        }
    }
}


