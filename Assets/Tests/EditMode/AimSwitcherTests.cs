using DemonCastle.Player;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace DemonCastle.Tests
{
    public class AimSwitcherTests
    {
        private class FakeAim : IAimInput
        {
            public Vector2 Direction { get; set; } = Vector2.right;

            public bool WasUsedThisFrame { get; set; }

            public void Tick(Vector2 playerPosition) { }
        }

        [Test]
        public async Task 생성자_처음에는_첫번째입력()
        {
            var first = new FakeAim();
            var second = new FakeAim { Direction = Vector2.up };
            var aimSwitcher = new AimSwitcher(first, second);

            Assert.AreSame(first, aimSwitcher.Current);
            Assert.AreEqual(Vector2.right, aimSwitcher.Direction);
        }

        [Test]
        public void Tick_두번째입력을건드리면_두번째로전환()
        {
            var first = new FakeAim { Direction = Vector2.right };
            var second = new FakeAim { Direction = Vector2.up };

            var aimSwitcher = new AimSwitcher(first, second);
            second.WasUsedThisFrame = true;

            aimSwitcher.Tick(Vector2.zero);

            Assert.AreSame(second, aimSwitcher.Current);
            Assert.AreEqual(Vector2.up, aimSwitcher.Direction);
        }

        [Test]
        public void Tick_아무도안건드리면_이전선택유지()
        {
            var first = new FakeAim();
            var second = new FakeAim { Direction = Vector2.up };
            var aimSwitcher = new AimSwitcher(first, second);

            second.WasUsedThisFrame = true;
            aimSwitcher.Tick(Vector2.zero);
            aimSwitcher.Tick(Vector2.zero);

            Assert.AreSame(second, aimSwitcher.Current);
            Assert.AreEqual(Vector2.up, aimSwitcher.Direction);
        }

        [Test]
        public void Tick_다시첫번째를건드리면_첫번째로복귀()
        {
            var first = new FakeAim();
            var second = new FakeAim { Direction = Vector2.up };
            var aimSwitcher = new AimSwitcher(first, second);

            second.WasUsedThisFrame = true;
            aimSwitcher.Tick(Vector2.zero);
            second.WasUsedThisFrame = false;

            first.WasUsedThisFrame = true;
            aimSwitcher.Tick(Vector2.zero);

            Assert.AreSame(first, aimSwitcher.Current);
            Assert.AreEqual(Vector2.right, aimSwitcher.Direction);
        }

        [Test]
        public void Tick_둘다건드리면() //"같은 프레임에 둘 다 건드리면 배열에서 뒤쪽이 우선한다."
        {
            var first = new FakeAim();
            var second = new FakeAim { Direction = Vector2.up };
            var aimSwitcher = new AimSwitcher(first, second);

            first.WasUsedThisFrame = true;
            second.WasUsedThisFrame = true;

            aimSwitcher.Tick(Vector2.zero);

            Assert.AreSame(second, aimSwitcher.Current);
            Assert.AreEqual(Vector2.up, aimSwitcher.Direction);
        }

        [Test]
        public void 생성자_입력이없으면_예외()
        {
            Assert.Throws<ArgumentException>(() => new AimSwitcher());
        }

    }
}

