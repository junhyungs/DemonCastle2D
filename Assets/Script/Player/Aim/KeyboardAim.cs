using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace DemonCastle.Player
{
    public class KeyboardAim : IAimInput
    {
        private readonly InputAction _aimKey;
        public Vector2 Direction { get; private set; } = Vector2.right;
        public bool WasUsedThisFrame { get; private set;  }

        public KeyboardAim(InputAction aimKey)
        {
            _aimKey = aimKey ?? throw new ArgumentNullException(nameof(aimKey));
        }

        public void Tick(Vector2 playerPosition)
        {
            Vector2 keyValue = _aimKey.ReadValue<Vector2>();
            WasUsedThisFrame = keyValue != Vector2.zero;
            if (WasUsedThisFrame) Direction = keyValue.normalized;
        }
    }
}

