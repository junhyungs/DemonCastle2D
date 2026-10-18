using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace DemonCastle.Save
{
    public class SaveService
    {
        private readonly string _filePath;
        private readonly string _tempPath;
        private readonly string _backupPath;
        private readonly string _corruptPath;

        public SaveService(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("세이브 파일 경로가 비어있습니다.", nameof(filePath));

            _filePath = filePath;
            _tempPath = _filePath + ".tmp";
            _backupPath = _filePath + ".bak";
            _corruptPath = _filePath + ".corrupt";
        }

        public void Save(SaveData saveData)
        {
            if (saveData == null) throw new ArgumentNullException(nameof(saveData));
            string json = JsonUtility.ToJson(saveData, prettyPrint: true);

            try
            {
                File.WriteAllText(_tempPath, json);
                if (File.Exists(_filePath)) File.Replace(_tempPath, _filePath, _backupPath);
                else File.Move(_tempPath, _filePath);
            }
            finally
            {
                if (File.Exists(_tempPath)) File.Delete(_tempPath);
            }
            
        }

        public SaveData Load()
        {
            var originFile = File.Exists(_filePath);

            var isNewGame = !originFile && !File.Exists(_backupPath);
            if (isNewGame) return new SaveData();

            if(TryRead(_filePath, out SaveData originData)) return originData;
            if(TryRead(_backupPath, out SaveData backUpData))
            {
                Debug.LogWarning($"[SaveService] '{_backupPath}' 이전 세이브 파일로 대체합니다");

                try
                {
                    if(originFile) File.Copy(_filePath, _corruptPath, overwrite: true);
                    File.Copy(_backupPath, _filePath, overwrite: true);                    
                }
                catch (IOException e)
                {
                    Debug.LogWarning($"[SaveService] 파일 복사 중 에러 : {e.Message}");                    
                }                
                return backUpData;
            }
            
            Debug.LogError($"[SaveService] 원본, 세이브 파일이 손상되었습니다 새 게임으로 시작합니다");
            return new SaveData();            
        }

        private static bool TryRead(string path, out SaveData data)
        {
            data = null;
            if (!File.Exists(path)) return false;

            try
            {
                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<SaveData>(json);

                if (data == null) return false;
                Sanitize(data);
                return true;
            }
            catch (ArgumentException e)
            {
                Debug.LogWarning($"[SaveService] '{path}' 파일이 손상되었습니다 : {e.Message}");
                return false;
            }
            catch (IOException e)
            {
                Debug.LogWarning($"[SaveService] '{path}' 파일을 읽지 못했습니다 : {e.Message}");
                return false;
            }
        }

        private static void Sanitize(SaveData data)
        {            
            data.WagePouches = Math.Max(0, data.WagePouches);
            data.ReturnCount = Math.Max(0, data.ReturnCount);
            data.StolenWagePouches = Math.Max(0, data.StolenWagePouches);
            data.UnlockedItemIds = data.UnlockedItemIds ?? new List<string>();
        }
    }
}

