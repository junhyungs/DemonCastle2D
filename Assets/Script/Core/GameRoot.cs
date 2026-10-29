using UnityEngine;
using System;
using System.IO;
using DemonCastle.Save;
using DemonCastle.Controls;
using DemonCastle.Data;

namespace DemonCastle.Core
{
    public class GameRoot : MonoBehaviour
    {
        [SerializeField] private GameDatabase _gameDatabase;
        private GameServices _services;
        private GameControls _controls;

        private async Awaitable Start()
        {
            DontDestroyOnLoad(gameObject);

            string savePath = Path.Combine(Application.persistentDataPath, "save.json");

            _controls = new GameControls();
            _controls.Player.Enable();

            if(_gameDatabase == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_gameDatabase)}");
            _gameDatabase.Validate();
            _services = new GameServices(new SceneLoader(), new SaveService(savePath), _controls, _gameDatabase);

            try
            {
                var saveService = _services.SaveService;
                SaveData data = saveService.Load();                
                Debug.Log($"[GameRoot] 세이브 불러옴, 귀환 횟수 : {data.ReturnCount}");

                var sceneLoader = _services.SceneLoader;
                await sceneLoader.LoadAsync(SceneNames.Hub, destroyCancellationToken);

                Debug.Log($"[GameRoot] {sceneLoader.CurrentScene} 로드완료");

                var entry = sceneLoader.FindEntry(SceneNames.Hub);
                entry.Initialize(_services);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("[GameRoot] 로딩이 취소되었습니다");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void OnDestroy()
        {
            _controls?.Dispose();
        }
    }
}


