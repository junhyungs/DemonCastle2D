using System;
using DemonCastle.Core;

namespace DemonCastle.Player
{
    public class RollAbility : ITickable
    {
        private readonly PlayerStats _stats;        
        private float _remainingTime;

        public RollAbility(PlayerStats stats)
        {
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));
        }

        public bool IsReady => _remainingTime <= 0f;

        public void StartCooldown()
        {            
            _remainingTime = _stats.RollCooldown;
        }
        
        public void Tick(float deltaTime)
        {            
            if(_remainingTime > 0f) _remainingTime -= deltaTime;
        }
    }
}

