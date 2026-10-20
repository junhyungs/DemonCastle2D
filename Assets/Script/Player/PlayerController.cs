using UnityEngine;
using System;
using DemonCastle.Controls;

namespace DemonCastle.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _aimIndicatorDistance = 0.8f;
        [SerializeField] private Transform _aimIndicator;
        private Rigidbody2D _rigidbody;
        private PlayerMovement _movement;
        private GameControls.PlayerActions _input;
        private IAimInput _aim;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            enabled = false;
        }

        private void FixedUpdate()
        {
            Vector2 input = _input.Move.ReadValue<Vector2>();
            _rigidbody.linearVelocity = _movement.CalculateVelocity(input);
        }

        private void Update()
        {
            _aim.Tick(transform.position);
            _aimIndicator.localPosition = _aim.Direction * _aimIndicatorDistance;
        }

        public void Initialize(GameControls.PlayerActions input, IAimInput aim)
        {
            if (_aimIndicator == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_aimIndicator)}");

            _input = input;
            _aim = aim ?? throw new ArgumentNullException(nameof(aim));
            _movement = new PlayerMovement(_moveSpeed);
            enabled = true;
        }
    }

}
