using NUnit.Framework;
using UnityEngine;
using DemonCastle.Combat;

namespace DemonCastle.Tests
{
    public class DamageInfoTests
    {
        [Test]
        public void 방향은_길이_1로_맞춰진다()
        {
            var info = new DamageInfo(1, new Vector2(3f, 4f), 4f);

            Assert.AreEqual(0.6f, info.Direction.x, 0.0001f);
            Assert.AreEqual(0.8f, info.Direction.y, 0.0001f);
        }

        [Test]
        public void 방향을_주지_않으면_밀지_않는다()
        {
            var info = new DamageInfo(1);

            Assert.AreEqual(Vector2.zero, info.Direction);
            Assert.AreEqual(0f, info.Knockback);
        }
    }
}