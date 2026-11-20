using System;
using UnityEngine;

namespace DemonCastle.Weapons
{
    [RequireComponent(typeof(Animator))]
    public sealed class AnimatorWeaponMotion : MonoBehaviour, IWeaponMotion
    {
        // Animator 창의 "상태" 이름이다(클립 이름이 아님). 무기를 바꿔도 상태 이름은 그대로라 해시가 계속 유효하다.
        // 4타짜리 무기가 생기면 여기에 "Swing4"를 추가하고 컨트롤러에도 상태를 만든다.
        private static readonly int IdleState = Animator.StringToHash("Idle");
        private static readonly int[] SwingStates =
        {
            Animator.StringToHash("Swing1"),
            Animator.StringToHash("Swing2"),
            Animator.StringToHash("Swing3")
        };

        private Animator _animator;

        public event Action Hit;
        public event Action ComboWindowOpened;
        public event Action Ended;        

        public void Initialize(RuntimeAnimatorController animations)
        {
            if (animations == null) throw new ArgumentNullException(nameof(animations));
            _animator = GetComponent<Animator>();
            _animator.runtimeAnimatorController = animations;
        }

        public void Play(int step)
        {
            if(step < 0 || step >= SwingStates.Length) throw new ArgumentOutOfRangeException(nameof(step), step, $"Swing 상태는 {SwingStates.Length}개까지만 준비되어 있습니다.");
            _animator.Play(SwingStates[step], 0, 0f);
            
        }

        public void Stop() => _animator.Play(IdleState, 0, 0f);

        // 클립의 애니메이션 이벤트가 "함수 이름 문자열"로 부른다.
        // 이름을 바꾸면 모든 클립의 이벤트도 같이 바꿔야 한다. (컴파일러가 잡아 주지 못함)
        public void OnHit() => Hit?.Invoke();
        public void OnComboWindow() => ComboWindowOpened?.Invoke();
        public void OnSwingEnd() => Ended?.Invoke();
    }
}

