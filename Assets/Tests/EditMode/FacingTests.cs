using NUnit.Framework;
using UnityEngine;
using DemonCastle.Player;

namespace DemonCastle.Tests
{
    public class FacingTests
    {
        [Test]
        public void 처음에는_오른쪽을_본다()
        {
            var facing = new Facing();

            Assert.AreEqual(1, facing.Sign);
            Assert.AreEqual(Vector2.right, facing.Forward);
        }

        [Test]
        public void 왼쪽을_조준하면_왼쪽으로_돌고_한번만_알린다()
        {
            var facing = new Facing();
            var changedCount = 0;
            facing.Changed += _ => changedCount++;

            facing.Follow(Vector2.left);
            facing.Follow(Vector2.left);

            Assert.AreEqual(-1, facing.Sign);
            Assert.AreEqual(1, changedCount);
        }

        [Test]
        public void 거의_수직이면_이전_방향을_유지한다()
        {
            var facing = new Facing();
            facing.Follow(Vector2.left);

            facing.Follow(new Vector2(0.05f, 1f).normalized);

            Assert.AreEqual(-1, facing.Sign);
        }

        [Test]
        public void 데드존을_넘으면_돈다()
        {
            var facing = new Facing();
            facing.Follow(Vector2.left);

            facing.Follow(new Vector2(0.2f, 1f).normalized);

            Assert.AreEqual(1, facing.Sign);
        }

        [Test]
        public void 잠그면_조준을_따라가지_않는다()
        {
            var facing = new Facing();
            facing.Lock(Vector2.right);

            facing.Follow(Vector2.left);

            Assert.AreEqual(1, facing.Sign);
        }

        [Test]
        public void 잠글_때_방향을_바꾸고_알린다()
        {
            var facing = new Facing();
            var changedCount = 0;
            facing.Changed += _ => changedCount++;

            facing.Lock(Vector2.left);

            Assert.AreEqual(-1, facing.Sign);
            Assert.AreEqual(1, changedCount);
        }

        [Test]
        public void ForwardFor는_계산만_하고_방향을_바꾸지_않는다()
        {
            var facing = new Facing();

            var forward = facing.ForwardFor(Vector2.left);

            Assert.AreEqual(Vector2.left, forward);
            Assert.AreEqual(1, facing.Sign);
        }
    }
}