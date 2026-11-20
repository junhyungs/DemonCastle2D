using UnityEngine;

namespace DemonCastle.Weapons
{
    [CreateAssetMenu(fileName = "WeaponDefinition", menuName = "DemonCastle/Weapon Definition")]
    public sealed class WeaponDefinition : ScriptableObject
    {
        [Header("공격")]
        [SerializeField, Min(1)] private int _damage = 1;
        [SerializeField, Min(0f)] private float _cooldown = 0.1f;

        [Header("범위")]
        [SerializeField, Min(0.1f)] private float _range = 1.2f;
        [SerializeField, Range(10f, 360f)] private float _arcDegrees = 120f;

        [Header("연출")]
        [SerializeField] private RuntimeAnimatorController _animations;
        [Tooltip("클립의 끝 이벤트가 오지 않을 때 강제로 끝내는 시간. 가장 긴 클립보다 길어야 한다.")]
        [SerializeField, Min(0.1f)] private float _swingTimeout = 1f;

        public int Damage => _damage;
        public float Cooldown => _cooldown;
        public float Range => _range;
        public float ArcDegrees => _arcDegrees;
        public RuntimeAnimatorController Animations => _animations;
        public float SwingTimeout => _swingTimeout;
    }
}

