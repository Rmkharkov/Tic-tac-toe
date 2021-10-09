namespace Game.Gameplay
{
    using Game.Cells;

    public class PickSellsLogic
    {
        static bool crossPlayerState = true;

        public static void RefreshTable()
        {
            crossPlayerState = true;
        }

        public static ECellState CurrentCellMark => crossPlayerState ? ECellState.Cross : ECellState.Circle;

        public static void CellTapped() => crossPlayerState = !crossPlayerState;
    }
}