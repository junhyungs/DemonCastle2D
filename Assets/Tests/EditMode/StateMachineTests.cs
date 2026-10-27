using NUnit.Framework;
using DemonCastle.Core;
using System;

namespace DemonCastle.Tests
{
    public class StateMachineTests
    {
        private enum TestStateId { A, B}
        private class FakeState : IState
        {
            public int EnterCount;
            public int ExitCount;
            public int TickCount;
            public void Enter()
            {
                EnterCount++;
            }

            public void Exit()
            {
                ExitCount++;
            }

            public void FixedTick(float fixedDeltaTime) { }

            public void Tick(float deltaTime)
            {
                TickCount++;
            }
        }

        [Test]
        public void ChangeState_처음이면_Enter만호출()
        {
            var stateMachine = new StateMachine<TestStateId>();
            var stateA = new FakeState();

            stateMachine.Register(TestStateId.A, stateA);            
            stateMachine.ChangeState(TestStateId.A);

            Assert.AreEqual(1, stateA.EnterCount);            
            Assert.AreEqual(0, stateA.ExitCount);
            Assert.AreEqual(TestStateId.A, stateMachine.CurrentId);
        }

        [Test]
        public void ChangeState_다른상태로바꾸면_이전Exit후다음Enter()
        {
            var stateMachine = new StateMachine<TestStateId>();
            var stateA = new FakeState();
            var stateB = new FakeState();

            stateMachine.Register(TestStateId.A, stateA);
            stateMachine.Register(TestStateId.B, stateB);

            stateMachine.ChangeState(TestStateId.A);
            stateMachine.ChangeState(TestStateId.B);

            Assert.AreEqual(1, stateA.ExitCount);
            Assert.AreEqual(1, stateB.EnterCount);
            Assert.AreEqual(TestStateId.B, stateMachine.CurrentId);
        }

        [Test]
        public void Tick_현재상태에만전달()
        {
            var stateMachine = new StateMachine<TestStateId>();
            var stateA = new FakeState();
            var stateB = new FakeState();

            stateMachine.Register(TestStateId.A, stateA);
            stateMachine.Register(TestStateId.B, stateB);

            stateMachine.ChangeState(TestStateId.A);
            stateMachine.Tick(0f);
            
            Assert.AreEqual(1, stateA.TickCount);
            Assert.AreEqual(0, stateB.TickCount);
        }

        [Test]
        public void Tick_상태가없으면_아무일없음()
        {
            var stateMachine = new StateMachine<TestStateId>();
            var stateA = new FakeState();
            var stateB = new FakeState();

            stateMachine.Register(TestStateId.A, stateA);
            stateMachine.Register(TestStateId.B, stateB);

            stateMachine.Tick(0f);

            Assert.AreEqual(0, stateA.TickCount);
            Assert.AreEqual(0, stateB.TickCount);
            Assert.IsFalse(stateMachine.HasState);
        }

        [Test]
        public void ChangeState_등록안된상태면_예외()
        {
            var stateMachine = new StateMachine<TestStateId>();
            Assert.Throws<InvalidOperationException>(() => stateMachine.ChangeState(TestStateId.A));
        }

        [Test]
        public void Register_같은상태를두번등록하면_예외()
        {
            var stateMachine = new StateMachine<TestStateId>();
            stateMachine.Register(TestStateId.A, new FakeState());
            Assert.Throws<InvalidOperationException>(() => stateMachine.Register(TestStateId.A, new FakeState()));
        }

        [Test]
        public void ChangeState_같은상태면_다시들어감()
        {
            var stateMachine = new StateMachine<TestStateId>();
            var stateA = new FakeState();
            var stateB = new FakeState();

            stateMachine.Register(TestStateId.A, stateA);
            stateMachine.Register(TestStateId.B, stateB);

            stateMachine.ChangeState(TestStateId.A);
            stateMachine.ChangeState(TestStateId.A);            

            Assert.AreEqual(TestStateId.A, stateMachine.CurrentId);
            Assert.AreEqual(2, stateA.EnterCount);
            Assert.AreEqual(1, stateA.ExitCount);
        }

        [Test]
        public void RegisterAll_모든값을등록()
        {
            var stateMachine = new StateMachine<TestStateId>();
            stateMachine.RegisterAll(id => new FakeState());
            stateMachine.ChangeState(TestStateId.A);
            stateMachine.ChangeState(TestStateId.B);

            Assert.AreEqual(TestStateId.B, stateMachine.CurrentId);
        }

        [Test]
        public void RegisterAll_null을주면_예외()
        {
            var stateMachine = new StateMachine<TestStateId>();
            Assert.Throws<InvalidOperationException>(() =>  stateMachine.RegisterAll(id => id == TestStateId.B ? null : new FakeState()));
        }
    }
}

