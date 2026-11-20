using System;
using System.Collections.Generic;
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
            public event Action ComboWindowOpened;
            public event Action Ended;

            public List<int> PlayedSteps { get; } = new List<int>();
            public int StopCount { get; private set; }

            public void Play(int step) => PlayedSteps.Add(step);
            public void Stop() => StopCount++;

            public void RaiseHit() => Hit?.Invoke();
            public void RaiseComboWindow() => ComboWindowOpened?.Invoke();
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
            _definition = ScriptableObject.CreateInstance<WeaponDefinition>();   // 기본값 = 검 3타
            _motion = new FakeWeaponMotion();
            _weapon = new MeleeWeapon(_definition, _owner.transform, _motion, LayerMask.GetMask("Enemy"));
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_owner);
            Object.DestroyImmediate(_definition);
        }

        // 1타부터 시작해 step번째(0부터) 타까지 콤보를 진행시킨다.
        private void AdvanceTo(int step)
        {
            _weapon.TryAttack(Vector2.right);
            for (var i = 0; i < step; i++)
            {
                _weapon.TryAttack(Vector2.right);
                _motion.RaiseComboWindow();
            }
        }

        [Test]
        public void 첫_입력은_1타를_재생한다()
        {
            Assert.IsTrue(_weapon.TryAttack(Vector2.right));

            CollectionAssert.AreEqual(new[] { 0 }, _motion.PlayedSteps);
            Assert.IsTrue(_weapon.IsAttacking);
        }

        [Test]
        public void 콤보_창_전_입력은_예약됐다가_창이_열리면_다음_타로_이어진다()
        {
            _weapon.TryAttack(Vector2.right);

            Assert.IsTrue(_weapon.TryAttack(Vector2.right));
            CollectionAssert.AreEqual(new[] { 0 }, _motion.PlayedSteps);   // 아직 2타 아님

            _motion.RaiseComboWindow();
            CollectionAssert.AreEqual(new[] { 0, 1 }, _motion.PlayedSteps);
        }

        [Test]
        public void 콤보_창이_열린_뒤_입력은_즉시_다음_타로_이어진다()
        {
            _weapon.TryAttack(Vector2.right);
            _motion.RaiseComboWindow();

            _weapon.TryAttack(Vector2.right);

            CollectionAssert.AreEqual(new[] { 0, 1 }, _motion.PlayedSteps);
        }

        [Test]
        public void 입력이_없으면_끝_이벤트에서_콤보가_끝나고_다음은_1타부터다()
        {
            _weapon.TryAttack(Vector2.right);
            _motion.RaiseComboWindow();
            _motion.RaiseEnded();
            Assert.IsFalse(_weapon.IsAttacking);

            _weapon.Tick(_definition.Cooldown + 0.01f);
            _weapon.TryAttack(Vector2.right);

            CollectionAssert.AreEqual(new[] { 0, 0 }, _motion.PlayedSteps);
        }

        [Test]
        public void 마지막_타에서는_추가_입력을_받지_않는다()
        {
            var last = _definition.Steps.Count - 1;
            AdvanceTo(last);

            Assert.IsFalse(_weapon.TryAttack(Vector2.right));
            Assert.AreEqual(last + 1, _motion.PlayedSteps.Count);
        }

        [Test]
        public void 다음_타는_예약한_순간의_방향으로_나간다()
        {
            _weapon.TryAttack(Vector2.right);
            _weapon.TryAttack(Vector2.left);

            _motion.RaiseComboWindow();

            Assert.AreEqual(Vector2.left, _weapon.AttackDirection);
        }

        [Test]
        public void 공격_중에는_그_타의_이동_배율을_쓴다()
        {
            Assert.AreEqual(1f, _weapon.MoveMultiplier);

            _weapon.TryAttack(Vector2.right);

            Assert.AreEqual(_definition.Steps[0].MoveMultiplier, _weapon.MoveMultiplier);
        }

        [Test]
        public void 취소하면_모션을_멈추고_콤보가_끝난다()
        {
            _weapon.TryAttack(Vector2.right);

            _weapon.Cancel();

            Assert.AreEqual(1, _motion.StopCount);
            Assert.IsFalse(_weapon.IsAttacking);
        }

        [Test]
        public void 끝_이벤트가_없어도_안전_시간이_지나면_끝난다()
        {
            _weapon.TryAttack(Vector2.right);

            _weapon.Tick(_definition.SwingTimeout + 0.01f);

            Assert.IsFalse(_weapon.IsAttacking);
            Assert.AreEqual(1, _motion.StopCount);
        }

        [Test]
        public void 공격_중이_아닐_때_온_이벤트는_무시한다()
        {
            _motion.RaiseComboWindow();
            _motion.RaiseEnded();
            _motion.RaiseHit();

            Assert.IsTrue(_weapon.IsReady);
            CollectionAssert.IsEmpty(_motion.PlayedSteps);
        }
    }
}