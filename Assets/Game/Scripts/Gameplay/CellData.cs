namespace Game.Cells
{
    [System.Serializable]
    public class CellData
    {
        public ICellItemView    CellItem;
        public ECellState       CellState { get; private set; }

        public void ChangeState(int setStateInt)
        {
            CellState = (ECellState)setStateInt;
            CellItem.Circle.SetActive(CellState == ECellState.Circle);
            CellItem.Cross.SetActive(CellState == ECellState.Cross);
            CellItem.CellButton.interactable = CellState == ECellState.Empty;
        }
    }
}