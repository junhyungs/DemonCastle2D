using UnityEngine;
using System;

namespace DemonCastle.Core
{
    public class GameRoot : MonoBehaviour
    {
        private SceneLoader _sceneLoader;

        private async Awaitable Start()
        {
            DontDestroyOnLoad(gameObject);

            _sceneLoader = new SceneLoader();

            try
            {
                await _sceneLoader.LoadAsync(SceneNames.Hub, destroyCancellationToken);                
                Debug.Log($"[GameRoot] {_sceneLoader.CurrentScene} 로드완료");
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


