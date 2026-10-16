using UnityEngine;
using DemonCastle.Core;
using System;

namespace DemonCastle.Hub
{
    public class HubExitDoor : MonoBehaviour
    {
        private SceneLoader _loader;
        public void Initialize(SceneLoader loader)
        {
            if (loader == null) throw new ArgumentException($"{nameof(loader)}가 전달되지않았습니다.");
            Debug.Log($"[HubExitDoor]{nameof(loader)}받음");
            _loader = loader;
        }
    }
}


