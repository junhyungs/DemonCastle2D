using System;
using UnityEngine;

namespace DemonCastle.Player
{
    [CreateAssetMenu(fileName = "PlayerDefinition", menuName = "DemonCastle/Player Definition")]
    public class PlayerDefinition : ScriptableObject
    {
        [Header("이동")]
        [SerializeField, Min(0.01f)] private float _moveSpeed = 5f;

        [Header("구르기")]
        [SerializeField, Min(0.01f)] private float _rollSpeed = 12f;
        [SerializeField, Min(0.01f)] private float _rollDuration = 0.25f;
        [SerializeField, Min(0f)] private float _rollCooldown = 0.4f;

        [Header("체력")]
        [SerializeField, Min(2)] private int _maxHealth = 6;
        [SerializeField, Min(0f)] private float _hitInvincibleDuration = 1f;

        public float MoveSpeed => _moveSpeed;
        public float RollSpeed => _rollSpeed;
        public float RollDuration => _rollDuration;
        public float RollCooldown => _rollCooldown;
        public int MaxHealth => _maxHealth;
        public float HitInvincibleDuration => _hitInvincibleDuration;

        public event Action Changed;
        private void OnValidate() => Changed?.Invoke();
    }
}

