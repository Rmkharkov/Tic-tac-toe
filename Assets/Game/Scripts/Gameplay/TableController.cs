namespace Game.Gameplay
{
    using System.Collections.Generic;
    using UnityEngine;
    using Game.Cells;
    using Game.Configs;
    using Game.Core;

    public interface ITableController
    {
        void OnCellPressed(CellData cellData);
        void Initialize();
        void CreateTable();
        List<CellData> Cells { get; }
    }

    public class TableController : MonoBehaviour, ITableController
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

#region ITableController

        public List<CellData>           Cells => _cells;

        public void OnCellPressed(CellData cellData)
        {
            cellData.ChangeState(PickSellsLogic.CurrentCellMark);
        }

        public void Initialize()
        {
            _tableView = TableView.Current;
            CreateTable();
        }

        public void CreateTable()
        {
            List<int> savedPreviosly = SaveSystem.Instance.Load().Cells;
            if (_cells != null && _cells.Count > 0)
            {
                _cells.ForEach(c => c.ChangeState(ECellState.Empty));
            }
            else
            {
                for (int i = 0; i < 9; i++)
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
                        PickSellsLogic.CellTapped(_cells[id]);
                    });

                    ECellState setState = ECellState.Empty;
                    if (savedPreviosly != null && savedPreviosly.Count > 0)
                    {
                        setState = (ECellState)savedPreviosly[i];
                    }
                    _cells[i].ChangeState(setState);
                }
                if (!SaveSystem.Instance.Load().CrossPlayerState)
                {
                    PickSellsLogic.AIMove();
                }
            }
        }

#endregion
    }
}