using UnityEngine;
using DemonCastle.Core;
using System;
using DemonCastle.Player;
using DemonCastle.UI;
using DemonCastle.Enemy;
using DemonCastle.Navigation;

namespace DemonCastle.Hub
{
    public class HubEntry : MonoBehaviour, ISceneEntry
    {
        [SerializeField] private HubExitDoor _exitDoor;
        [SerializeField] private PlayerController _player;
        [SerializeField] private Camera _camera;
        [SerializeField] private HealthView _healthView;

        // TODO(10교시): 스포너가 생기면 삭제. 허브에는 원래 적이 없다.
        [Header("임시 적 테스트")]
        [SerializeField] private EnemyController[] _sandboxEnemies;
        [SerializeField] private EnemyDefinition _sandboxEnemyDefinition;

        private HealthViewModel _healthViewModel;
        private GameServices _services;

        public void Initialize(GameServices services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (_exitDoor == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_exitDoor)}");
            if (_player == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_player)}");
            if(_camera == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_camera)}");
            if(_healthView == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_healthView)}");

            _services = services;

            var sceneLoader = _services.SceneLoader;
            Debug.Log($"[HubEntry] 서비스 받음. 현재 씬 : {sceneLoader.CurrentScene}");
            
            _exitDoor.Initialize(sceneLoader);

            var gameControls = _services.Controls;
            var playerInput = gameControls.Player;
            var aim = new AimSwitcher(
                    new MouseAim(playerInput.AimPoint, _camera),
                    new KeyboardAim(playerInput.AimKey)
                );

            var gameDataBase = _services.Database;
            _player.Initialize(playerInput, aim, gameDataBase.Player);

            _healthViewModel = new HealthViewModel(_player.Health);
            _healthView.Bind(_healthViewModel);

            var navigator = new DirectNavigator();
            foreach(var enemy in _sandboxEnemies)
            {
                if (enemy == null) throw new InvalidOperationException($"{nameof(_sandboxEnemies)}에 빈 칸이 있습니다.");
                enemy.Initialize(_sandboxEnemyDefinition, _player.BodyCollider, navigator);
            }
        }

        private void OnDestroy()
        {
            _healthViewModel?.Dispose();
        }
    }
}

