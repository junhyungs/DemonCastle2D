using NUnit.Framework;
using DemonCastle.Player;

namespace DemonCastle.Tests
{
    public class RollAbilityTests
    {
        [Test]
        public void 처음_만들면_준비됨()
        {            
            var rollAbility = InitializeRollAbility(1f, 1f, 1f, 1f);
            Assert.IsTrue(rollAbility.IsReady);
        }

        [Test]
        public void 시작하면_준비_안_됨()
        {
            var rollAbility = InitializeRollAbility(1f, 1f, 1f, 1f);
            rollAbility.StartCooldown();
            Assert.IsFalse(rollAbility.IsReady);
        }

        [Test]
        public void 시작_후_쿨다운_시간만큼_Tick하면_준비됨()
        {
            var rollAbility = InitializeRollAbility(1f, 1f, 1f, 1f);
            rollAbility.StartCooldown();
            Assert.IsFalse(rollAbility.IsReady);

            rollAbility.Tick(1f);
            Assert.IsTrue(rollAbility.IsReady);
        }

        [Test]
        public void 시작_후_쿨다운보다_짧게_Tick하면_여전히_준비_안_됨()
        {
            var rollAbility = InitializeRollAbility(1f, 1f, 1f, 1f);
            rollAbility.StartCooldown();
            Assert.IsFalse(rollAbility.IsReady);

            rollAbility.Tick(0.5f);
            Assert.IsFalse(rollAbility.IsReady);
        }

        [Test]
        public void 쿨다운이_0이면_시작해도_바로_준비됨()
        {
            var rollAbility = InitializeRollAbility(1f, 1f, 1f, 0f);
            rollAbility.StartCooldown();           
            Assert.IsTrue(rollAbility.IsReady);
        }

        private static RollAbility InitializeRollAbility(float moveSpeed, float rollSpeed, float rollDuration, float rollCooldown)
        {
            var playerStats = new PlayerStats(moveSpeed, rollSpeed, rollDuration, rollCooldown);
            return new RollAbility(playerStats);
        }
    }
}

