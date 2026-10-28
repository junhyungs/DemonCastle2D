using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DemonCastle.Tests
{
    public class ClosureLearningTests
    {
        [Test]
        public void For문_람다는_반복변수를_붙잡는다()
        {
            var actions = new List<Action>();
            var results = new List<int>();

            for(int i = 0; i < 3; i++)
            {
                int index = i;
                actions.Add(() => results.Add(index));
            }

            foreach(var action in actions)
            {
                action.Invoke();
            }

            CollectionAssert.AreEqual(new[] { 0, 1, 2 }, results);
        }
    }
}

