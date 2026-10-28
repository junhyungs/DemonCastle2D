using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonCastle.UI
{
    public class HealthView : View<HealthViewModel>
    {
        [SerializeField] private Image _heartPrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private Sprite _fullSprite;
        [SerializeField] private Sprite _halfSprite;
        [SerializeField] private Sprite _emptySprite;

        private List<Image> _hearts = new List<Image>();

        protected override void OnBind(HealthViewModel viewModel)
        {
            if (_heartPrefab == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_heartPrefab)}");
            if (_container == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_container)}");

            viewModel.Hearts.Changed += Render;
            Render(viewModel.Hearts.Value);
        }

        protected override void OnUnbind(HealthViewModel viewModel)
        {
            viewModel.Hearts.Changed -= Render;
        }

        private void Render(HeartState[] states)
        {
            while(_hearts.Count < states.Length)
            {
                _hearts.Add(Instantiate(_heartPrefab, _container));
            }

            for(int i = 0; i < _hearts.Count; i++)
            {
                bool isUsed = i < states.Length;
                _hearts[i].gameObject.SetActive(isUsed);

                if (isUsed)
                {
                    _hearts[i].sprite = ForSprite(states[i]);
                }
            }
        }

        private Sprite ForSprite(HeartState state)
        {
            return state switch
            {
                HeartState.Full => _fullSprite,
                HeartState.Half => _halfSprite,
                HeartState.Empty => _emptySprite,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, "알 수 없는 하트 상태입니다.")
            };
        }
    }
}

