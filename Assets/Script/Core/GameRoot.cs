using UnityEngine;
using System;
using DemonCastle.Hub;

namespace DemonCastle.Core
{
    public class GameRoot : MonoBehaviour
    {
        private GameServices _services;

        private async Awaitable Start()
        {
            DontDestroyOnLoad(gameObject);

            _services = new GameServices(new SceneLoader());

            try
            {
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


