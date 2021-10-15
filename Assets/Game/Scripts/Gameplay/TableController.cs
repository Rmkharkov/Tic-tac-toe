namespace Game.Gameplay
{
    using System.Collections.Generic;
    using UnityEngine;
    using Game.Cells;
    using Game.Configs;
    using Game.Core;
    using UniRx;
    using System;

    public interface ITableController
    {
        void Initialize();
        void CreateTable();
        List<CellData> Cells { get; }
    }

    public class TableController : MonoBehaviour, ITableController, IDisposable
    {
        private static ITableController  _instance;
        public static ITableController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TableController();
                }
                return _instance;
            }
        }

        private ITableView              _tableView;

        private List<CellData>          _cells = new List<CellData>();

        SaveData                        _currentSave => SaveProfile.Instance.SaveData;

#region ITableController

        public List<CellData>           Cells => _cells;

        public void Initialize()
        {
            _tableView = TableView.Current;
            CreateTable();
        }

        public void CreateTable()
        {
            if (_cells != null && _cells.Count > 0)
            {
                _cells.ForEach(c => c.ChangeState((int)ECellState.Empty));
            }
            else
            {
                for (int i = 0; i < _currentSave.Cells.Length; i++)
                {
                    ICellItemView cell = Instantiate(
                        PrefabsLoaderConfig.Instance.GetPrefab(Prefabs.EGameplayPrefab.Cell), 
                        _tableView.CellsParent)
                        .GetComponent<ICellItemView>();

                    _cells.Add(new CellData()
                    {
                        CellItem = cell,
                    });

                    int id = i;
                    _cells[i].CellItem.CellButton.onClick.AddListener(delegate {
                        PickSellsLogic.CellTapped(id);
                    });
                    _currentSave.Cells[id]
                        .Subscribe(_cells[id].ChangeState)
                        .AddTo(_lifetimeDisposables);
                }
                if (!_currentSave.CrossPlayerState.Value)
                {
                    PickSellsLogic.AIMove();
                }
            }
        }

        public void Dispose() => _lifetimeDisposables.Clear();

#endregion

        readonly CompositeDisposable _lifetimeDisposables = new CompositeDisposable();

    }
}