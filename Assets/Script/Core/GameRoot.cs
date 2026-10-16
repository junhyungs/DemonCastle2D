using UnityEngine;
using System;
using DemonCastle.Hub;
using System.IO;
using DemonCastle.Save;

namespace DemonCastle.Core
{
    public class GameRoot : MonoBehaviour
    {
        private GameServices _services;

        private async Awaitable Start()
        {
            DontDestroyOnLoad(gameObject);

            string savePath = Path.Combine(Application.persistentDataPath, "save.json");
            _services = new GameServices(new SceneLoader(), new SaveService(savePath));

            try
            {
                var saveService = _services.SaveService;
                SaveData data = saveService.Load();
                data.ReturnCount++;
                saveService.Save(data);
                Debug.Log($"[GameRoot] 귀환 횟수 : {data.ReturnCount}, 저장 위치 : {savePath}");

                var sceneLoader = _services.SceneLoader;
                await sceneLoader.LoadAsync(SceneNames.Hub, destroyCancellationToken);

                Debug.Log($"[GameRoot] {sceneLoader.CurrentScene} 로드완료");

                HubEntry hubEntry = FindAnyObjectByType<HubEntry>();
                if (hubEntry == null) throw new InvalidOperationException($"{SceneNames.Hub} 씬에 HubEntry가 없습니다.");
                hubEntry.Initialize(_services);
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
    }
}


