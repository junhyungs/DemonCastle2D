using UnityEngine;

namespace DemonCastle.Enemy
{
    [CreateAssetMenu(fileName = "EnemyDefinition", menuName = "DemonCastle/Enemy Definition")]
    public class EnemyDefinition : ScriptableObject
    {
        [Header("이동")]
        [SerializeField, Min(0.01f)] private float _wanderSpeed = 1.5f;
        [SerializeField, Min(0.01f)] private float _chaseSpeed = 3.5f;

        [Header("감지")]
        [SerializeField, Min(0.01f)] private float _detectRange = 4f;
        [SerializeField, Min(0.01f)] private float _loseRange = 7f;
        [SerializeField, Min(0f)] private float _stopDistance = 0.6f;

        [Header("방황")]
        [SerializeField, Min(0f)] private float _wanderRadius = 2f;
        [SerializeField, Min(0f)] private float _idleTimeMin = 1f;
        [SerializeField, Min(0f)] private float _idleTimeMax = 3f;

        [Header("전투")]
        [SerializeField, Min(1)] private int _maxHealth = 3;
        [SerializeField, Min(0f)] private float _hitInvincibleDuration = 0f;
        [SerializeField, Min(1)] private int _contactDamage = 1;
        [Tooltip("맞은 뒤 밀려나며 행동하지 못하는 시간(초). 이 시간 동안 넉백 속도가 0까지 줄어든다.")]
        [SerializeField, Min(0.05f)] private float _hurtDuration = 0.25f;

        [Header("무리")]
        [SerializeField, Min(0.0f)] private float _separationRadius = 1.5f;
        [SerializeField, Min(0f)] private float _separationWeight = 3f;

        public float WanderSpeed => _wanderSpeed;
        public float ChaseSpeed => _chaseSpeed;
        public float DetectRange => _detectRange;
        public float LoseRange => _loseRange;
        public float WanderRadius => _wanderRadius;
        public float IdleTimeMin => _idleTimeMin;
        public float IdleTimeMax => _idleTimeMax;
        public int MaxHealth => _maxHealth;
        public float HurtDuration => _hurtDuration;
        public float HitInvincibleDuration => _hitInvincibleDuration;
        public int ContactDamage => _contactDamage;
        public float StopDistance => _stopDistance;
        public float SeparationRadius => _separationRadius;
        public float SeparationWeight => _separationWeight;

        private void OnValidate()
        {
            _loseRange = Mathf.Max(_loseRange, _detectRange);
            _idleTimeMax = Mathf.Max(_idleTimeMax, _idleTimeMin);
            _stopDistance = Mathf.Min(_stopDistance, _detectRange);
        }
    }
}

