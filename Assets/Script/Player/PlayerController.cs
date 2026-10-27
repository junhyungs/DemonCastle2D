using UnityEngine;
using System;
using DemonCastle.Controls;

namespace DemonCastle.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Min(0.01f)] private float _moveSpeed = 5f;
        [SerializeField, Min(0.01f)] private float _rollSpeed = 12f;
        [SerializeField, Min(0.01f)] private float _rollDuration = 0.25f;
        [SerializeField, Min(0f)] private float _rollCooldown = 0.4f;
        [SerializeField] private float _aimIndicatorDistance = 0.8f;
        [SerializeField] private Transform _aimIndicator;
        [SerializeField] private PlayerStateId _currentStateForDebug;

        private Rigidbody2D _rigidbody;
        private PlayerStats _stats;
        private PlayerBrain _brain;
        private IAimInput _aim;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            enabled = false;
        }

        private void FixedUpdate()
        {
            _brain.FixedTick(Time.fixedDeltaTime);
        }

        private void Update()
        {
            _aim.Tick(transform.position);
            _aimIndicator.localPosition = _aim.Direction * _aimIndicatorDistance;

            _brain.Tick(Time.deltaTime);
            _currentStateForDebug = _brain.CurrentState;
        }

        private void OnValidate()
        {
            if (_stats == null) return;
            _stats.MoveSpeed = _moveSpeed;
            _stats.RollSpeed = _rollSpeed;
            _stats.RollDuration = _rollDuration;
            _stats.RollCooldown = _rollCooldown;
        }

        public void Initialize(GameControls.PlayerActions input, IAimInput aim)
        {
            if (_aimIndicator == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_aimIndicator)}");
            
            _aim = aim ?? throw new ArgumentNullException(nameof(aim));
            _stats = new PlayerStats(_moveSpeed, _rollSpeed, _rollDuration, _rollCooldown);
            _brain = new PlayerBrain(input, _rigidbody, _aim, _stats);
            enabled = true;
        }
    }

}
