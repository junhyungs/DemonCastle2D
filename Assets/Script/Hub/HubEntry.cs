using UnityEngine;
using DemonCastle.Core;
using System;

namespace DemonCastle.Hub
{
    public class HubEntry : MonoBehaviour
    {
        [SerializeField] private HubExitDoor _exitDoor;
        private GameServices _services;

        public void Initialize(GameServices services)
        {
            if (services == null) throw new ArgumentNullException($"[HubEntry]{nameof(services)}가 전달되지않았습니다.");
            if (_exitDoor == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_exitDoor)}");

            _services = services;
            Debug.Log($"[HubEntry] 서비스 받음. 현재 씬 : {_services.SceneLoader.CurrentScene}");
            
            _exitDoor.Initialize(_services.SceneLoader);
        }
    }
}

