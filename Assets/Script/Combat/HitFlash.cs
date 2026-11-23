using UnityEngine;
using System;

namespace DemonCastle.Combat
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class HitFlash : MonoBehaviour
    {
        [SerializeField] private Color _flashColor = new Color(1f, 0.35f, 0.35f);
        [SerializeField, Min(0.01f)] private float _duration = 0.1f;

        private SpriteRenderer _renderer;
        private Color _originalColor;
        private Health _health;
        private float _remainingTime;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _originalColor = _renderer.color;
            enabled = false;
        }

        public void Initialize(Health health)
        {
            _health = health ?? throw new ArgumentNullException(nameof(health));
            _health.Damaged += OnDamaged;
        }

        private void OnDestroy()
        {
            if (_health != null) _health.Damaged -= OnDamaged;
        }

        private void OnDamaged(DamageInfo _)
        {
            _remainingTime = _duration;
            _renderer.color = _flashColor;
            enabled = true;
        }

        private void Update()
        {
            _remainingTime -= Time.deltaTime;
            if (_remainingTime > 0f) return;

            _renderer.color = _originalColor;
            enabled = false;
        }
    }
}

