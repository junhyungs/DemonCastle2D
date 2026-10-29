using DemonCastle.Player;
using System;
using UnityEngine;

namespace DemonCastle.Data
{
    [CreateAssetMenu(fileName = "GameDatabase", menuName = "DemonCastle/Game Database")]
    public class GameDatabase : ScriptableObject
    {
        [Header("PlayerDefinition")]
        [SerializeField] private PlayerDefinition _player;

        public PlayerDefinition Player => _player;

        public void Validate()
        {
            if(_player == null) throw new InvalidOperationException($"GameDatabase에 {nameof(_player)}가 연결되지 않았습니다.");
        }
    }
}
