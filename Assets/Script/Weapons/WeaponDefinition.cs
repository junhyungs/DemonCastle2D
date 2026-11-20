using System.Collections.Generic;
using UnityEngine;

namespace DemonCastle.Weapons
{
    [CreateAssetMenu(fileName = "WeaponDefinition", menuName = "DemonCastle/Weapon Definition")]
    public sealed class WeaponDefinition : ScriptableObject
    {
        [Header("콤보")]
        [Tooltip("콤보 순서대로 1타, 2타 ... Animator의 상태 Swing1, Swing2 ... 와 짝을 이룬다")]
        [SerializeField] private AttackStep[] _steps =
        {
            new AttackStep(1, 1.2f, 200f, 0.5f),
            new AttackStep(1, 1.2f, 200f, 0.5f),
            new AttackStep(2, 1.5f, 270f, 0.2f)
        };
        [Tooltip("콤보가 끝난 뒤, 다음 콤보까지 대기 시간")]
        [SerializeField, Min(0f)] private float _cooldown = 0.1f;

        [Header("연출")]
        [SerializeField] private RuntimeAnimatorController _animations;
        [Tooltip("클립의 끝 이벤트가 오지 않을 때 강제로 끝내는 시간. 가장 긴 클립보다 길어야 한다.")]
        [SerializeField, Min(0.1f)] private float _swingTimeout = 1f;        
        
        public IReadOnlyList<AttackStep> Steps => _steps;        
        public float Cooldown => _cooldown;        
        public RuntimeAnimatorController Animations => _animations;
        public float SwingTimeout => _swingTimeout;
    }
}

