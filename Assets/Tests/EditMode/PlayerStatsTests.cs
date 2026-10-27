using NUnit.Framework;
using System;
using DemonCastle.Player;

namespace DemonCastle.Tests
{
    public class PlayerStatsTests
    {
        [Test]
        public void 생성자_이동속도가0이하면_예외()
        {            
            Assert.Throws<ArgumentOutOfRangeException>(() => new PlayerStats(0, 1f, 1f, 1f));
        }

        [Test]
        public void 생성자_쿨다운0은_허용()
        {
            var playerStats = new PlayerStats(1f, 1f, 1f, 0);
            Assert.AreEqual(0, playerStats.RollCooldown, 0.0001f);
        }

        [Test]
        public void RollCooldown_음수면_예외()
        {
            var newStats = new PlayerStats(1f, 1f, 1f, 1f);
            Assert.Throws<ArgumentOutOfRangeException>(() => newStats.RollCooldown = -1f);
        }

        [Test]
        public void MoveSpeed_바꾸면_새값을돌려줌()
        {
            var playerStats = new PlayerStats(1f, 1f, 1f, 1f);
            Assert.AreEqual(1f, playerStats.MoveSpeed);

            playerStats.MoveSpeed = 2f;
            Assert.AreEqual(2f, playerStats.MoveSpeed);
        }
    }
}

