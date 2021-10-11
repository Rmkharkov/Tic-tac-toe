namespace Game.Core
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Game.Cells;
    using Game.Gameplay;
    using System.Linq;

    [System.Serializable]
    public class SaveData
    {
        public List<int>        Cells = new List<int>();
        public bool             CrossPlayerState = true;
        public bool             GameEndState = true;
    }

    public interface ISaveSystem
    {
        void Save();
        void RefreshSave();
        SaveData Load();
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

        private SaveData _loadedData = null;
        private string _saveId = "gameSave";

        public void Save()
        {
            _loadedData.Cells.Clear();
            TableController.Instance.Cells
                .ForEach(c => _loadedData.Cells.Add((int)c.CellState));
            
            _loadedData.CrossPlayerState = PickSellsLogic.crossPlayerState;
            PPSerialization.Save(_saveId, _loadedData);
        }

        public void RefreshSave()
        {
            _loadedData = new SaveData();
            PPSerialization.Save(_saveId, _loadedData);
        }

        public SaveData Load()
        {
            if (PlayerPrefs.HasKey(_saveId))
            {
                _loadedData = PPSerialization.Load<SaveData>(_saveId);
            }
            else
            {
                _loadedData = new SaveData();
            }

            PickSellsLogic.crossPlayerState = _loadedData.CrossPlayerState;

            return _loadedData;
        }
    }
}