using DemonCastle.UI;
using NUnit.Framework;
using System;

namespace DemonCastle.Tests
{
    public class HeartCalculatorTests
    {
        [TestCase(6, 6, new[] { HeartState.Full, HeartState.Full, HeartState.Full })]
        [TestCase(5, 6, new[] { HeartState.Full, HeartState.Full, HeartState.Half })]
        [TestCase(3, 6, new[] { HeartState.Full, HeartState.Half, HeartState.Empty })]
        [TestCase(0, 6, new[] { HeartState.Empty, HeartState.Empty, HeartState.Empty })]
        public void Heart계산테스트(int current, int max, HeartState[] testArray)
        {
            CollectionAssert.AreEqual(testArray, HeartCalculator.Calculate(current, max));
        }

        [Test]
        public void 홀수최대체력예외()
        {
            Assert.Throws<ArgumentException>(() => HeartCalculator.Calculate(1, 3));
        }
    }
}

