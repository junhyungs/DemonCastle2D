using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DemonCastle.Core
{
    public class SceneLoader
    {
        public string CurrentScene { get; private set; }

        public async Awaitable LoadAsync(string sceneName, CancellationToken token)
        {            
            var isSceneNameNullOrEmpty = string.IsNullOrEmpty(sceneName);
            if (isSceneNameNullOrEmpty) throw new ArgumentException("SceneName IsNullOrEmpty", nameof(sceneName));

            var canLoad = Application.CanStreamedLevelBeLoaded(sceneName);
            if (!canLoad) throw new InvalidOperationException("This scene name is not unregistered");

            token.ThrowIfCancellationRequested();
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            while (!operation.isDone)
            {
                await Awaitable.NextFrameAsync(token);
            }

            CurrentScene = sceneName;
        }
    }
}

