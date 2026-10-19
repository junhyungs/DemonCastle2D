using UnityEngine;
using DemonCastle.Core;
using System;
using DemonCastle.Player;

namespace DemonCastle.Hub
{
    public class HubEntry : MonoBehaviour, ISceneEntry
    {
        [SerializeField] private HubExitDoor _exitDoor;
        [SerializeField] private PlayerController _player;
        private GameServices _services;

        public void Initialize(GameServices services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (_exitDoor == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_exitDoor)}");
            if (_player == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_player)}");

            _services = services;
            Debug.Log($"[HubEntry] 서비스 받음. 현재 씬 : {_services.SceneLoader.CurrentScene}");
            
            _exitDoor.Initialize(_services.SceneLoader);
            _player.Initialize(_services.Controls.Player);
        }
    }
}

