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
            if (loader == null) throw new ArgumentNullException(nameof(loader));            
            _loader = loader;
            Debug.Log($"[HubExitDoor]{nameof(loader)}받음");
        }
    }
}


