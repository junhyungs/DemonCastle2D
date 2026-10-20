using UnityEngine;
using System;

namespace DemonCastle.Player
{
    public class AimSwitcher : IAimInput
    {
        private readonly IAimInput[] _sources;
        private IAimInput _current;

        public Vector2 Direction => _current.Direction;
        public bool WasUsedThisFrame => _current.WasUsedThisFrame;
        public IAimInput Current => _current;

        public AimSwitcher(params IAimInput[] sources)
        {
            if (sources == null || sources.Length == 0) throw new ArgumentException("조준 입력이 최소 하나는 필요합니다.", nameof(sources));
            _sources = sources;
            _current = sources[0];
        }

        public void Tick(Vector2 playerPosition) // 같은 프레임에 여러 입력을 건드리면 배열 뒤쪽이 우선한다
        {
            foreach(var source in _sources)
            {
                source.Tick(playerPosition);
                if(source.WasUsedThisFrame) _current = source;
            }
        }
    }
}

