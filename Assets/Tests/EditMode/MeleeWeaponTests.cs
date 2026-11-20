using System;
using NUnit.Framework;
using UnityEngine;
using DemonCastle.Weapons;
using Object = UnityEngine.Object;

namespace DemonCastle.Tests
{
    public class MeleeWeaponTests
    {
        private sealed class FakeWeaponMotion : IWeaponMotion
        {
            public event Action Hit;
            public event Action Ended;
            public int PlayCount { get; private set; }

            public void Play() => PlayCount++;
            public void RaiseHit() => Hit?.Invoke();
            public void RaiseEnded() => Ended?.Invoke();
        }

        private GameObject _owner;
        private WeaponDefinition _definition;
        private FakeWeaponMotion _motion;
        private MeleeWeapon _weapon;

        [SetUp]
        public void SetUp()
        {
            _owner = new GameObject("Owner");
            _definition = ScriptableObject.CreateInstance<WeaponDefinition>();
            _motion = new FakeWeaponMotion();
            _weapon = new MeleeWeapon(_definition, _owner.transform, _motion, LayerMask.GetMask("Enemy"));
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_owner);
            Object.DestroyImmediate(_definition);
        }

        [Test]
        public void 공격하면_모션을_재생하고_휘두르는_동안은_다시_공격할_수_없다()
        {
            Assert.IsTrue(_weapon.TryAttack(Vector2.right));
            Assert.IsFalse(_weapon.TryAttack(Vector2.right));

            Assert.AreEqual(1, _motion.PlayCount);
            Assert.IsFalse(_weapon.IsReady);
        }

        [Test]
        public void 끝_이벤트_뒤_쿨다운이_지나면_다시_공격할_수_있다()
        {
            _weapon.TryAttack(Vector2.right);

            _motion.RaiseEnded();
            _weapon.Tick(_definition.Cooldown + 0.01f);

            Assert.IsTrue(_weapon.IsReady);
        }

        [Test]
        public void 끝_이벤트가_없어도_안전_시간이_지나면_끝난다()
        {
            _weapon.TryAttack(Vector2.right);

            _weapon.Tick(_definition.SwingTimeout + 0.01f);   // 강제 종료 → 쿨다운 시작
            _weapon.Tick(_definition.Cooldown + 0.01f);       // 쿨다운 소진

            Assert.IsTrue(_weapon.IsReady);
        }

        [Test]
        public void 공격_중이_아닐_때_온_끝_이벤트는_무시한다()
        {
            _motion.RaiseEnded();

            Assert.IsTrue(_weapon.IsReady);
        }
    }
}