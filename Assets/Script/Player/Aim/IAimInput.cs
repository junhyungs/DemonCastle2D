using UnityEngine;

namespace DemonCastle.Player
{
    public interface IAimInput
    {
        Vector2 Direction { get; }
        bool WasUsedThisFrame { get; }
        void Tick(Vector2 playerPosition);
    }
}


