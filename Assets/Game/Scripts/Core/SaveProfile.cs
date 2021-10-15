namespace Game.Core
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UniRx;
    using System.Linq;

    [System.Serializable]
    public class SaveData
    {
        public IntReactiveProperty[] Cells = new IntReactiveProperty[9];
        public BoolReactiveProperty CrossPlayerState    = new BoolReactiveProperty();
    }

    public interface ISaveProfile
    {
        SaveData    SaveData { get; }
        void        SetData(SaveData saveData);
        void        ReSetData();
    }

    public class SaveProfile : MonoBehaviour, ISaveProfile
    {
        private static ISaveProfile _instance;
        public static ISaveProfile Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SaveProfile();
                }
                return _instance;
            }
        }

        private SaveData        _saveData;

        public SaveData         SaveData => _saveData;

        public void SetData(SaveData saveData)
        {
            _saveData = saveData;
        }

        public void ReSetData()
        {
            _saveData = new SaveData();
            _saveData.CrossPlayerState.Value = true;
            for (int i = 0; i < _saveData.Cells.Length; i++)
                _saveData.Cells[i] = new IntReactiveProperty(1);
        }
    }
}