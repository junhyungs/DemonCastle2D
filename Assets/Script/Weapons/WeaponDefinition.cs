using UnityEngine;

namespace DemonCastle.Weapons
{
    [CreateAssetMenu(fileName = "WeaponDefinition", menuName = "DemonCastle/Weapon Definition")]
    public sealed class WeaponDefinition : ScriptableObject
    {
        [Header("공격")]
        [SerializeField, Min(1)] private int _damage = 1;
        [SerializeField, Min(0.05f)] private float _cooldown = 0.35f;

        [Header("범위")]
        [SerializeField, Min(0.1f)] private float _range = 1.2f;
        [SerializeField, Range(10f, 360f)] private float _arcDegrees = 120f;

        [Header("연출")]
        [SerializeField, Min(0.01f)] private float _swingDuration = 0.15f;

        public int Damage => _damage;
        public float Cooldown => _cooldown;
        public float Range => _range;
        public float ArcDegrees => _arcDegrees;
        public float SwingDuration => _swingDuration;

        private void OnValidate()
        {
            //휘두르는 연출이 다음 공격보다 길면 연출이 중간에 끊김.
            _swingDuration = Mathf.Min(_swingDuration, _cooldown);
        }
    }
}

