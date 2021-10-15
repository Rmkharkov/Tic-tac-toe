namespace Game.Core
{
    using UnityEngine;
    using SavingCore;

    public interface ISaveSystem
    {
        void        Save();
        void        RefreshSave();
        SaveData    Load();
    }

    public class SaveSystem : MonoBehaviour, ISaveSystem
    {
        private static ISaveSystem _instance;
        public static ISaveSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SaveSystem();
                }
                return _instance;
            }
        }

        private SaveData    _loadedData = null;
        private string      _saveId = "gameSave";

#region ISaveSystem

        SaveData            _currentSave => SaveProfile.Instance.SaveData;

        public void Save()
        {
            PPSerialization.Save(_saveId, _currentSave);
        }

        public void RefreshSave()
        {
            SaveProfile.Instance.ReSetData();
            PPSerialization.Save(_saveId, _currentSave);
        }

        public SaveData Load()
        {
            if (PlayerPrefs.HasKey(_saveId))
            {
                SaveProfile.Instance.SetData(PPSerialization.Load<SaveData>(_saveId));
            }
            else
            {
                SaveProfile.Instance.ReSetData();
            }

            return _currentSave;
        }

#endregion
    }
}