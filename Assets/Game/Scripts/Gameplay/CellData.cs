namespace Game.Gameplay
{
    using Game.Cells;

    public class CellData
    {
        public ICellItemView CellItem;
        public ECellState debState;
        public ECellState CellState { get; private set; }

        public void ChangeState(ECellState setState)
        {
            CellState = setState;
            CellItem.Circle.SetActive(setState == ECellState.Circle);
            CellItem.Cross.SetActive(setState == ECellState.Cross);
            CellItem.CellButton.interactable = setState == ECellState.Empty;
            debState = setState;
        }
    }
}