using System;
using System.Collections.Generic;

namespace DemonCastle.Save
{
    [Serializable]
    public class SaveData
    {
        public const int CurrentVersion = 1;

        public int Version = CurrentVersion;
        public int WagePouches;
        public int ReturnCount;
        public int StolenWagePouches;
        public List<string> UnlockedItemIds = new List<string>();
    }
}


