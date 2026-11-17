using UnityEngine;
using NUnit.Framework;
using DemonCastle.Navigation;

namespace DemonCastle.Tests
{
    public class DirectNavigatorTests
    {
        private const float Delta = 0.0001f;        

        [Test]
        public void 오른쪽_목표면_오른쪽_방향()
        {
            var directNavigator = new DirectNavigator();
            var direction = directNavigator.GetDirection(Vector2.zero, new Vector2(5f, 0f));

            Assert.AreEqual(Vector2.right.x, direction.x, Delta);
            Assert.AreEqual(Vector2.right.y, direction.y, Delta);
        }

        [Test]
        public void 멀리_있어도_길이는_1()
        {
            var directNavigator = new DirectNavigator();
            var direction = directNavigator.GetDirection(Vector2.zero, new Vector2(30f, -40f));

            Assert.AreEqual(1f, direction.magnitude, Delta);
        }

        [Test]
        public void 출발점과_목표가_같으면_영벡터()
        {
            var directNavigator = new DirectNavigator();
            var direction = directNavigator.GetDirection(new Vector2(5f, 12f), new Vector2(5f, 12f));

            Assert.AreEqual(Vector2.zero.x, direction.x, Delta);
            Assert.AreEqual(Vector2.zero.y, direction.y, Delta);
        }
    }
}

