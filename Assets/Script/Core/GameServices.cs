using System;

namespace DemonCastle.Core
{
    public class GameServices
    {
        public SceneLoader SceneLoader { get; }

        public GameServices(SceneLoader loader)
        {
            SceneLoader = loader ?? throw new ArgumentNullException(nameof(loader));
        }
    }
}

