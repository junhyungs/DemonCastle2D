using System;

namespace DemonCastle.UI
{
    public static class HeartCalculator
    {
        public const int HpPerHeart = 2;
        public static HeartState[] Calculate(int current, int max)
        {
            if (max <= 0) throw new ArgumentException($"최대 체력은 0, 음수가 될 수 없습니다.", nameof(max));
            if (max % HpPerHeart != 0) throw new ArgumentException($"최대 체력은 짝수여야합니다", nameof(max));

            var heartCount = max / HpPerHeart;
            HeartState[] heartStates = new HeartState[heartCount];

            var remainingHp = Math.Clamp(current, 0, max);
            for(int i = 0; i < heartCount; i++)
            {
                if(remainingHp >= HpPerHeart)
                {
                    heartStates[i] = HeartState.Full;
                    remainingHp -= HpPerHeart;
                } 
                else if(remainingHp == 1)
                {
                    heartStates[i] = HeartState.Half;
                    remainingHp -= 1;
                }
                else
                {
                    heartStates[i] = HeartState.Empty;
                }
            }

            return heartStates;
        }
    }
}


