using UnityEngine;
using System;
using DemonCastle.Combat;

namespace DemonCastle.UI
{
    public class HealthViewModel : IDisposable
    {
        private readonly Health _health;
        private readonly ObservableValue<HeartState[]> _hearts;

        public IReadOnlyObservableValue<HeartState[]> Hearts => _hearts;
        public HealthViewModel(Health health)
        {
            _health = health ?? throw new ArgumentNullException(nameof(health));
            _hearts = new ObservableValue<HeartState[]>(HeartCalculator.Calculate(health.Current, health.Max));
            _health.Changed += OnHealthChanged;
        }

        private void OnHealthChanged(int current, int max)
        {
            _hearts.Value = HeartCalculator.Calculate(current, max);
        }

        public void Dispose()
        {
            _health.Changed -= OnHealthChanged;
        }
    }
}

