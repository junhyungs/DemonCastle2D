using System;
using System.IO;
using UnityEngine;

namespace DemonCastle.Save
{
    public class SaveService
    {
        private readonly string _filePath;
        public SaveService(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("세이브 파일 경로가 비어있습니다.", nameof(filePath));
            _filePath = filePath;
        }

        public void Save(SaveData saveData)
        {
            if (saveData == null) throw new ArgumentNullException(nameof(saveData));
            string json = JsonUtility.ToJson(saveData, prettyPrint: true);
            File.WriteAllText(_filePath, json);
        }

        public SaveData Load()
        {
            if (!File.Exists(_filePath)) return new SaveData();
            string json = File.ReadAllText(_filePath);
            return JsonUtility.FromJson<SaveData>(json);
        }
    }
}

