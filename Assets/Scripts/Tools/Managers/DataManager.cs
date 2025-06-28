using UnityEngine;
using System.Collections.Generic;
using System.IO;
using GK.Platform;

namespace GK.Managers
{
    /// <summary>
    /// 数据管理器
    /// 统一处理本地数据存储，适配不同平台的存储机制
    /// </summary>
    public static class DataManager
    {
        private static Dictionary<string, object> _cache = new Dictionary<string, object>();
        private static bool _useEncryption = false;
        private static string _encryptionKey = "GameDataKey2024";

        /// <summary>
        /// 是否使用加密
        /// </summary>
        public static bool UseEncryption
        {
            get => _useEncryption;
            set => _useEncryption = value;
        }

        /// <summary>
        /// 设置加密密钥
        /// </summary>
        public static void SetEncryptionKey(string key)
        {
            _encryptionKey = key;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public static void SaveData<T>(string key, T data)
        {
            try
            {
                string jsonData = JsonUtility.ToJson(data);

                if (_useEncryption)
                {
                    jsonData = EncryptString(jsonData);
                }

                // 根据平台选择存储方式
                if (PlatformDetector.IsMiniGame)
                {
                    SaveToPlayerPrefs(key, jsonData);
                }
                else
                {
                    SaveToFile(key, jsonData);
                }

                // 更新缓存
                _cache[key] = data;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save data for key '{key}': {e.Message}");
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public static T LoadData<T>(string key, T defaultValue = default(T))
        {
            try
            {
                // 先检查缓存
                if (_cache.TryGetValue(key, out object cachedData))
                {
                    return (T)cachedData;
                }

                string jsonData = "";

                // 根据平台选择加载方式
                if (PlatformDetector.IsMiniGame)
                {
                    jsonData = LoadFromPlayerPrefs(key);
                }
                else
                {
                    jsonData = LoadFromFile(key);
                }

                if (string.IsNullOrEmpty(jsonData))
                {
                    return defaultValue;
                }

                if (_useEncryption)
                {
                    jsonData = DecryptString(jsonData);
                }

                T data = JsonUtility.FromJson<T>(jsonData);

                // 更新缓存
                _cache[key] = data;

                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load data for key '{key}': {e.Message}");
                return defaultValue;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        public static void DeleteData(string key)
        {
            try
            {
                // 从缓存中移除
                _cache.Remove(key);

                // 根据平台删除数据
                if (PlatformDetector.IsMiniGame)
                {
                    PlayerPrefs.DeleteKey(key);
                }
                else
                {
                    string filePath = GetFilePath(key);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to delete data for key '{key}': {e.Message}");
            }
        }

        /// <summary>
        /// 检查数据是否存在
        /// </summary>
        public static bool HasData(string key)
        {
            // 先检查缓存
            if (_cache.ContainsKey(key))
            {
                return true;
            }

            // 根据平台检查数据
            if (PlatformDetector.IsMiniGame)
            {
                return PlayerPrefs.HasKey(key);
            }
            else
            {
                return File.Exists(GetFilePath(key));
            }
        }

        /// <summary>
        /// 清除所有数据
        /// </summary>
        public static void ClearAllData()
        {
            try
            {
                _cache.Clear();

                if (PlatformDetector.IsMiniGame)
                {
                    PlayerPrefs.DeleteAll();
                }
                else
                {
                    string dataPath = GetDataPath();
                    if (Directory.Exists(dataPath))
                    {
                        Directory.Delete(dataPath, true);
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to clear all data: {e.Message}");
            }
        }

        /// <summary>
        /// 保存到PlayerPrefs
        /// </summary>
        private static void SaveToPlayerPrefs(string key, string data)
        {
            PlayerPrefs.SetString(key, data);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 从PlayerPrefs加载
        /// </summary>
        private static string LoadFromPlayerPrefs(string key)
        {
            return PlayerPrefs.GetString(key, "");
        }

        /// <summary>
        /// 保存到文件
        /// </summary>
        private static void SaveToFile(string key, string data)
        {
            string filePath = GetFilePath(key);
            string directory = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filePath, data);
        }

        /// <summary>
        /// 从文件加载
        /// </summary>
        private static string LoadFromFile(string key)
        {
            string filePath = GetFilePath(key);

            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }

            return "";
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        private static string GetFilePath(string key)
        {
            return Path.Combine(GetDataPath(), $"{key}.dat");
        }

        /// <summary>
        /// 获取数据存储路径
        /// </summary>
        private static string GetDataPath()
        {
            return Path.Combine(Application.persistentDataPath, "GameData");
        }

        /// <summary>
        /// 简单字符串加密
        /// </summary>
        private static string EncryptString(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            byte[] data = System.Text.Encoding.UTF8.GetBytes(text);
            byte[] key = System.Text.Encoding.UTF8.GetBytes(_encryptionKey);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(data[i] ^ key[i % key.Length]);
            }

            return System.Convert.ToBase64String(data);
        }

        /// <summary>
        /// 简单字符串解密
        /// </summary>
        private static string DecryptString(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
                return encryptedText;

            try
            {
                byte[] data = System.Convert.FromBase64String(encryptedText);
                byte[] key = System.Text.Encoding.UTF8.GetBytes(_encryptionKey);

                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = (byte)(data[i] ^ key[i % key.Length]);
                }

                return System.Text.Encoding.UTF8.GetString(data);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取缓存大小
        /// </summary>
        public static int GetCacheSize()
        {
            return _cache.Count;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public static void ClearCache()
        {
            _cache.Clear();
        }

        /// <summary>
        /// 获取所有缓存的键
        /// </summary>
        public static string[] GetCachedKeys()
        {
            string[] keys = new string[_cache.Count];
            _cache.Keys.CopyTo(keys, 0);
            return keys;
        }

        /// <summary>
        /// 批量保存数据
        /// </summary>
        public static void SaveBatch(Dictionary<string, object> dataDict)
        {
            foreach (var kvp in dataDict)
            {
                SaveData(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// 获取数据存储信息
        /// </summary>
        public static string GetStorageInfo()
        {
            string platform = PlatformDetector.IsMiniGame ? "PlayerPrefs" : "File System";
            return $"Storage Platform: {platform}, " +
                   $"Cache Size: {GetCacheSize()}, " +
                   $"Encryption: {UseEncryption}, " +
                   $"Data Path: {GetDataPath()}";
        }
    }
}
