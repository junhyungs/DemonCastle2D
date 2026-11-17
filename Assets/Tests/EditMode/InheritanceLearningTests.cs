using NUnit.Framework;

namespace DemonCastle.Tests
{
    public class InheritanceLearningTests
    {
        private abstract class Monster
        {
            protected Monster()
            {
                OnCreated();
            }

            protected abstract void OnCreated();
        }

        private class Skeleton : Monster
        {
            private readonly string _name;
            private readonly string _kind = "언데드";

            public string CapturedName;
            public string CapturedKind;

            public Skeleton(string name)
            {
                _name = name;
            }

            protected override void OnCreated()
            {
                CapturedName = _name;
                CapturedKind = _kind;
            }
        }

        [Test]
        public void 부모_생성자의_가상호출_시점에는_자식_생성자가_아직_실행되지_않았다()
        {
            //필드 초기화는 자식이 먼저 수행되지만 
            //생성자는 자식이 부모 생성자보다 늦게 호출된다.
            //아래 코드는 이에 대한 예시를 보여주고 있다.
            //순서 -> 
            //자식 필드 -> 부모 필드 -> 부모 생성자 -> 자식 생성자             
            var skeleton = new Skeleton("해골");
            Assert.IsNull(skeleton.CapturedName);
            Assert.AreEqual("언데드", skeleton.CapturedKind);
        }
    }
}

