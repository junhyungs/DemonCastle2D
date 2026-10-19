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
            if (!canLoad) throw new InvalidOperationException($"This scene name is not registered[{sceneName}]");

            token.ThrowIfCancellationRequested();
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            while (!operation.isDone)
            {
                await Awaitable.NextFrameAsync(token);
            }

            CurrentScene = sceneName;
        }

        public ISceneEntry FindEntry(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.isLoaded) throw new InvalidOperationException($"'{sceneName}' 씬이 로드되어 있지 않습니다.");

            foreach(var root in scene.GetRootGameObjects())
            {
                if (root.TryGetComponent(out ISceneEntry entry)) return entry;
            }
            throw new InvalidOperationException($"'{sceneName}' 씬의 최상위 오브젝트에 ISceneEntry가 없습니다.");
        }
    }
}

