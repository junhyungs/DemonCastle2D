using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace DemonCastle.Player
{
    public class MouseAim : IAimInput
    {
        private readonly InputAction _aimPoint;
        private readonly Camera _camera;
        private Vector2 _lastScreenPosition;

        public Vector2 Direction { get; private set; } = Vector2.right;
        public bool WasUsedThisFrame { get; private set;  }

        public MouseAim(InputAction aimPoint, Camera camera)
        {
            _aimPoint = aimPoint ?? throw new ArgumentNullException(nameof(aimPoint));
            _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
        }

        public void Tick(Vector2 playerPosition)
        {
            Vector2 screenPosition = _aimPoint.ReadValue<Vector2>();
            WasUsedThisFrame = screenPosition != _lastScreenPosition;
            _lastScreenPosition = screenPosition;

            Vector2 worldPosition = _camera.ScreenToWorldPoint(screenPosition);
            Vector2 toMouse = worldPosition - playerPosition;
            if (toMouse.sqrMagnitude > 0.0001f) Direction = toMouse.normalized;

        }
    }
}

