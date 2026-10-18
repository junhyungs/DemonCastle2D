using System.Text.RegularExpressions;
using System.IO;
using DemonCastle.Save;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DemonCastle.Tests
{
    public class SaveServiceTests
    {
        private string _directory;
        private string _filePath;        

        [SetUp]
        public void SetUp()
        {
            _directory = Path.Combine(Path.GetTempPath(), "DemonCastleTests", Path.GetRandomFileName());

            Directory.CreateDirectory(_directory);
            _filePath = Path.Combine(_directory, "save.json");            
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_directory)) Directory.Delete(_directory, recursive: true);
        }

        [Test]
        public void Load_파일이없으면_새게임()
        {
            var service = new SaveService(_filePath);
            SaveData data = service.Load();

            Assert.AreEqual(0, data.ReturnCount);
            Assert.AreEqual(SaveData.CurrentVersion, data.Version);
        }

        [Test]
        public void SaveThenLoad_값이그대로()
        {
            var service = new SaveService(_filePath);
            var original = new SaveData { WagePouches = 7, ReturnCount = 3 };
            original.UnlockedItemIds.Add("rune_fire");

            service.Save(original);
            SaveData loaded = service.Load();

            Assert.AreEqual(7, loaded.WagePouches);
            Assert.AreEqual(3, loaded.ReturnCount);
            CollectionAssert.AreEqual(new[] { "rune_fire" }, loaded.UnlockedItemIds);
        }

        [Test]
        public void Load_본파일이깨지면_백업에서복구()
        {
            var service = new SaveService(_filePath);
            var targetData = new SaveData();

            SaveWithReturnCount(service, targetData, 1);
            SaveWithReturnCount(service, targetData, 2);
            File.WriteAllText(_filePath, "{ ReturnCount = }");

            SaveData loaded = service.Load();            
            Assert.AreEqual(1, loaded.ReturnCount);
        }

        [Test]
        public void Load_본파일이깨지면_corrupt에보관()
        {
            var service = new SaveService(_filePath);
            var targetData = new SaveData();
            
            SaveWithReturnCount(service, targetData, 1);
            SaveWithReturnCount(service, targetData, 2);

            File.WriteAllText(_filePath, "{ ReturnCount = }");

            SaveData loaded = service.Load();

            var corruptPath = _filePath + ".corrupt";
            bool isCorruptFile = File.Exists(corruptPath);
            string corruptString = File.ReadAllText(corruptPath);
            Assert.AreEqual(true, isCorruptFile);
            Assert.AreEqual("{ ReturnCount = }", corruptString);
        }

        [Test]
        public void Load_본파일이깨지고백업이없으면_새게임()
        {
            var service = new SaveService(_filePath);
            var targetData = new SaveData();

            SaveWithReturnCount(service, targetData, 1);

            File.WriteAllText(_filePath, "{ ReturnCount = }");
            LogAssert.Expect(LogType.Error, new Regex("세이브 파일이 손상되었습니다 새 게임으로 시작합니다"));

            SaveData loaded = service.Load();
            Assert.AreEqual(0, loaded.ReturnCount); 
        }

        [Test]
        public void Load_본파일이없으면_백업에서복구()
        {
            var service = new SaveService(_filePath);
            var targetData = new SaveData();

            SaveWithReturnCount(service, targetData, 1);
            SaveWithReturnCount(service, targetData, 2);

            File.Delete(_filePath);

            SaveData loaded = service.Load();
            Assert.AreEqual(1, loaded.ReturnCount);
        }

        [Test]
        public void Load_둘다깨지면_새게임()
        {
            var service = new SaveService(_filePath);
            var targetData = new SaveData();

            SaveWithReturnCount(service, targetData, 1);
            SaveWithReturnCount(service, targetData, 2);

            string backupPath = _filePath + ".bak";
            File.WriteAllText(_filePath, "{ ReturnCount = }");
            File.WriteAllText(backupPath, "{ ReturnCount = }");

            LogAssert.Expect(LogType.Error, new Regex("세이브 파일이 손상되었습니다 새 게임으로 시작합니다"));

            SaveData loaded = service.Load();
            Assert.AreEqual(0, loaded.ReturnCount);
        }

        [Test]
        public void Load_음수값은_0으로()
        {
            var service = new SaveService(_filePath);
            var targetData = new SaveData();

            SaveWithReturnCount(service, targetData, 1);
            File.WriteAllText(_filePath, "{\"WagePouches\": -5 }");

            var wageData = service.Load();
            Assert.AreEqual(0, wageData.WagePouches);
        }

        [Test]
        public void Save_두번저장하면_백업에이전값()
        {
            var service = new SaveService(_filePath);
            var targetData = new SaveData();

            SaveWithReturnCount(service, targetData, 1);            
            SaveWithReturnCount(service, targetData, 2);

            string backupPath = _filePath + ".bak";
            var backupJson = File.ReadAllText(backupPath);
            var backupData = JsonUtility.FromJson<SaveData>(backupJson);

            Assert.AreEqual(1, backupData.ReturnCount);
        }

        private static void SaveWithReturnCount(SaveService service, SaveData targetData, int point)
        {
            targetData.ReturnCount = point;
            service.Save(targetData);
        }        
    }
}


