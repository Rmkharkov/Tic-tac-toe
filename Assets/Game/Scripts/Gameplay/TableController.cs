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
        void                Initialize();
        List<CellData>      Cells { get; }
        ReactiveCommand     TableReset { get; }
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

        readonly CompositeDisposable    _lifetimeDisposables = new CompositeDisposable();

#region ITableController

        public List<CellData>           Cells => _cells;

        public ReactiveCommand          TableReset { get; } = new ReactiveCommand();

        public void Initialize()
        {
            _tableView = TableView.Current;
            TableReset.Subscribe(_ => CreateTable()).AddTo(_lifetimeDisposables);
            TableReset.Execute();
        }

#endregion

        public void Dispose() => _lifetimeDisposables.Clear();

        private void CreateTable()
        {
            _cells.Clear();
            foreach (Transform child in _tableView.CellsParent)
            {
                Destroy(child.gameObject);
            }

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
}