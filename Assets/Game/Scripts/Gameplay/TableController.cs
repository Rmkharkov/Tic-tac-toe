namespace Game.Gameplay
{
    using System.Collections.Generic;
    using UnityEngine;
    using Game.Cells;
    using Game.Configs;

    public class TableController : MonoBehaviour
    {
        public static TableController   Current;

        [SerializeField]
        private TableView               _tableView;

        private class CellData
        {
            public ICellItemView    CellItem;
            public ECellState       CellState { get; private set; }

            public void ChangeState(ECellState setState)
            {
                CellState = setState;
                CellItem.Circle.SetActive           (setState == ECellState.Circle);
                CellItem.Cross.SetActive            (setState == ECellState.Cross);
                CellItem.CellButton.interactable =  setState == ECellState.Empty;
            }
        }

        [SerializeField]
        private List<CellData>          Cells = new List<CellData>();

        private void Awake()
        {
            Current = this;
        }

        private void Start()
        {
            CreateTable();
        }

        void CreateTable()
        {
            if (Cells != null && Cells.Count > 0)
            {
                Cells.ForEach(c => c.ChangeState(ECellState.Empty));
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    ICellItemView cell = Instantiate(
                        PrefabsLoaderConfig.Instance.GetPrefab(Prefabs.EGameplayPrefab.Cell), 
                        _tableView.CellsParent)
                        .GetComponent<ICellItemView>();

                    Cells.Add(new CellData()
                    {
                        CellItem = cell,
                    });

                    int id = i;
                    Cells[i].CellItem.CellButton.onClick.AddListener(delegate {
                        OnCellPressed(Cells[id]);
                        PickSellsLogic.CellTapped();
                    });

                    Cells[i].ChangeState(ECellState.Empty);
                }
            }
        }

        void OnCellPressed(CellData cellData)
        {
            cellData.ChangeState(PickSellsLogic.CurrentCellMark);
        }
    }
}