using UnityEngine;
using DemonCastle.Controls;

namespace DemonCastle.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        private Rigidbody2D _rigidbody;
        private PlayerMovement _movement;
        private GameControls.PlayerActions _input;

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

        public void Initialize(GameControls.PlayerActions input)
        {
            _input = input;
            _movement = new PlayerMovement(_moveSpeed);
            enabled = true;
        }
    }

}
