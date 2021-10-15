namespace SavingCore
{
    using UnityEngine;
    using System.Collections.Generic;

    public class PPSerialization : MonoBehaviour
    {
        private static Dictionary<string, string> cachedData = new Dictionary<string, string>();

        public static string GetJsonDataFromPrefs(string saveTag)
        {
            string outData;
            if (cachedData.TryGetValue(saveTag, out outData))
            {
                if (!string.IsNullOrEmpty(outData))
                {
                    return outData;
                }
            }

            try
            {
                string temp = PlayerPrefs.GetString(saveTag);
                if (string.IsNullOrEmpty(temp))
                {
                    return temp;
                }
                return Encryption.Decrypt(temp);
            }
            catch (System.Exception e)
            {
                Debug.Log("GetJsonDataFromPrefs Exception" + e.Message + "  saveTag: " + saveTag);
                return null;
            }
        }

        public static void ClearCachedSavesData()
        {
            cachedData.Clear();
        }

        public static void Save<T>(string saveTag, T obj)
        {
            string json = JsonUtility.ToJson(obj);

            if (cachedData.ContainsKey(saveTag))
            {
                cachedData[saveTag] = json;
            }
            else
            {
                cachedData.Add(saveTag, json);
            }
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("Empty save data json saveTag: " + saveTag);
            }

            if (string.IsNullOrEmpty(json))
            {
                Debug.Log("<color=red>Null json</color> " + saveTag);
            }

            // add encryption here
            json = Encryption.Encrypt(json);

            PlayerPrefs.SetString(saveTag, json);
        }

        /// <summary>
        ///This type of saving will be bypassing saves data caching ( so cached save data will be wrong ).
        ///If it is not desired behaviour call ClearCachedSavesData method after.
        /// </summary>
        public static void Save(string saveTag, string jsonString)
        {
            if (cachedData.ContainsKey(saveTag))
            {
                cachedData[saveTag] = jsonString;
            }
            else
            {
                cachedData.Add(saveTag, jsonString);
            }
            string encrypted = Encryption.Encrypt(jsonString);
            PlayerPrefs.SetString(saveTag, encrypted);
        }

        public static T Load<T>(string saveTag, T defaultValue = null) where T : class
        {
            string outData;
            if (cachedData.TryGetValue(saveTag, out outData))
            {
                if (!string.IsNullOrEmpty(outData))
                {
                    return JsonUtility.FromJson<T>(outData);
                }
            }

            try
            {
                string temp = PlayerPrefs.GetString(saveTag);
                if (string.IsNullOrEmpty(temp))
                {
                    return defaultValue;
                }
                temp = Encryption.Decrypt(temp);
                return GetDataWithValidation<T>(temp);
            }
            catch (System.Exception e)
            {
                Debug.LogFormat("Load Exception {0} saveTag: {1}", e.Message, saveTag);
                if (defaultValue == null)
                {
                    defaultValue = default(T);
                }
                return defaultValue;
            }
        }

        private static T GetDataWithValidation<T>(string dataString) where T : class
        {
            var result = JsonUtility.FromJson<T>(dataString);
            if (result is ArrayInClassWrapper)
            {
                (result as ArrayInClassWrapper).ValidateInnerArraySize();
            }
            return result;
        }

    }
}