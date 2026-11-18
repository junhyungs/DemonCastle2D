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
        
        public int Damage => _damage;
        public float Cooldown => _cooldown;
        public float Range => _range;
        public float ArcDegrees => _arcDegrees;                
    }
}

