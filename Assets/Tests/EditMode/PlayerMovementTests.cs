using DemonCastle.Player;
using NUnit.Framework;
using UnityEngine;

namespace DemonCastle.Tests
{
    public class PlayerMovementTests
    {
        [Test]
        public void CalculateVelocity_입력이없으면_정지()
        {
            var velocity = CalculateVelocity(5f, Vector2.zero);
            Assert.AreEqual(Vector2.zero, velocity);
        }

        [Test]
        public void CalculateVelocity_오른쪽이면_속도만큼()
        {
            var velocity = CalculateVelocity(5f, Vector2.right);
            Assert.AreEqual(5f, velocity.x, 0.0001f);
            Assert.AreEqual(0f, velocity.y, 0.0001f);
        }

        [Test]
        public void CalculateVelocity_대각선도_속도가같음()
        {
            var velocity = CalculateVelocity(5f, Vector2.one);
            Assert.AreEqual(5f, velocity.magnitude, 0.0001f);
        }

        [Test]
        public void CalculateVelocity_스틱을살짝밀면_느리게()
        {
            var velocity = CalculateVelocity(5f, new Vector2(0.3f, 0f));
            Assert.AreEqual(1.5f, velocity.magnitude, 0.0001f);
        }

        private static Vector2 CalculateVelocity(float speed, Vector2 input)
        {            
            return PlayerMovement.CalculateVelocity(input, speed);
        }
    }

}
