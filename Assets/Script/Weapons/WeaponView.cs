using DemonCastle.Player;
using UnityEngine;
using System;

namespace DemonCastle.Weapons
{
    public sealed class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;

        private IWeapon _weapon;
        private IAimInput _aim;
        private WeaponDefinition _definition;

        private bool _isSwinging;
        private float _swingElapsed;
        private float _swingCenterAngle;
        private float _swingSign = 1f;

        private void Update()
        {
            if (_aim == null) return;
            if (!_isSwinging)
            {
                var aimAngle = ToAngle(_aim.Direction);
                SetAngle(aimAngle);
                return;
            }

            _swingElapsed += Time.deltaTime;
            var t = Mathf.Clamp01(_swingElapsed / _definition.SwingDuration);
            var half = _definition.ArcDegrees * 0.5f;
            SetAngle(_swingCenterAngle + Mathf.Lerp(half, -half, t) * _swingSign);

            if(t >= 1f)
            {
                _isSwinging = false;
                _swingSign = -_swingSign;
            }
        }

        private void OnDestroy()
        {
            if (_weapon != null) _weapon.Attacked -= OnAttacked;
        }

        public void Initialize(IWeapon weapon, IAimInput aim, WeaponDefinition definition)
        {
            if (_pivot == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_pivot)}");

            _weapon = weapon ?? throw new ArgumentNullException(nameof(weapon));
            _aim = aim ?? throw new ArgumentNullException(nameof(aim));
            _definition = definition != null ? definition : throw new ArgumentNullException(nameof(definition));

            _weapon.Attacked += OnAttacked;
        }

        private void OnAttacked(Vector2 direction)
        {
            _swingCenterAngle = ToAngle(direction);
            _swingElapsed = 0f;
            _isSwinging = true;
        }

        private void SetAngle(float degrees) => _pivot.localRotation = Quaternion.Euler(0f, 0f, degrees);
        private static float ToAngle(Vector2 direction) => Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}

