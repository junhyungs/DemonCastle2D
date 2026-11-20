using System;
using UnityEngine;

namespace DemonCastle.Weapons
{
    [RequireComponent(typeof(Animator))]
    public sealed class AnimatorWeaponMotion : MonoBehaviour, IWeaponMotion
    {
        private static readonly int SwingState = Animator.StringToHash("Swing");
        private Animator _animator;

        public event Action Hit;
        public event Action Ended;

        public void Initialize(RuntimeAnimatorController animations)
        {
            if (animations == null) throw new ArgumentNullException(nameof(animations));
            _animator = GetComponent<Animator>();
            _animator.runtimeAnimatorController = animations;
        }

        public void Play()
        {
            _animator.Play(SwingState, 0, 0f);
        }

        // 클립의 애니메이션 이벤트가 "함수 이름 문자열"로 부른다.
        // 이름을 바꾸면 모든 클립의 이벤트도 같이 바꿔야 한다. (컴파일러가 잡아 주지 못함)
        public void OnHit() => Hit?.Invoke();
        public void OnSwingEnd() => Ended?.Invoke();
    }
}

