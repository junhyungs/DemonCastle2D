using System;
using DemonCastle.Save;
using DemonCastle.Controls;
using DemonCastle.Data;

namespace DemonCastle.Core
{
    public class GameServices
    {
        public SceneLoader SceneLoader { get; }
        public SaveService SaveService { get; }
        public GameControls Controls { get; }
        public GameDatabase Database { get; }

        public GameServices(
            SceneLoader loader,
            SaveService saveService,
            GameControls controls,
            GameDatabase database
            )
        {
            SceneLoader = loader ?? throw new ArgumentNullException(nameof(loader));
            SaveService = saveService ?? throw new ArgumentNullException(nameof(saveService));
            Controls = controls ?? throw new ArgumentNullException(nameof(controls));
            Database = database ?? throw new ArgumentNullException(nameof(database));
        }
    }
}

