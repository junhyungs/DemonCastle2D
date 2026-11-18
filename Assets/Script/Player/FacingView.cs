using UnityEngine;
using System;

namespace DemonCastle.Player
{
    public sealed class FacingView : MonoBehaviour
    {
        private Facing _facing;

        public void Initialize(Facing facing)
        {
            _facing = facing ?? throw new ArgumentNullException(nameof(facing));
            _facing.Changed += Apply;
            Apply(_facing.Sign);
        }

        private void OnDestroy()
        {
            if(_facing != null) _facing.Changed -= Apply;
        }

        private void Apply(int sign)
        {
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * sign;
            transform.localScale = scale;
        }
    }
}

