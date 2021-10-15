namespace Game.Gameplay
{
    using Game.Cells;
    using Game.UI;
    using Game.AI;
    using Game.Core;
    using System.Linq;
    using System.Collections.Generic;

    public class PickSellsLogic
    {
        static bool someOneWon = false;
        static SaveData _currentSave => SaveProfile.Instance.SaveData;

        public static void RefreshTable()
        {
            _currentSave.CrossPlayerState.Value = true;
            someOneWon = false;
            TableController.Instance.CreateTable();
            UIController.Instance.ChangePlayer(CurrentCellMark);
        }

        public static ECellState CurrentCellMark => _currentSave.CrossPlayerState.Value ? ECellState.Cross : ECellState.Circle;

        public static void CellTapped(int cellDataId)
        {
            _currentSave.Cells[cellDataId].Value = (int)CurrentCellMark;

            _currentSave.CrossPlayerState.Value = !_currentSave.CrossPlayerState.Value;

            UIController.Instance.ChangePlayer(CurrentCellMark);

            if (!CheckTableEndState)
            {
                SaveSystem.Instance.Save();
                AIMove();
            }
        }

        public static void AIMove()
        {
            if (!_currentSave.CrossPlayerState.Value && !NoEmptyCells && !someOneWon)
            {
                AIController.Instance.CanMove();
            }
        }

        private static bool CheckTableEndState
        {
            get
            {
                if (ThreeInARow != ECellState.Empty)
                {
                    UIController.Instance.GetWinner(ThreeInARow);
                    someOneWon = true;
                    SaveSystem.Instance.RefreshSave();
                    return true;
                }
                else if (NoEmptyCells)
                {
                    UIController.Instance.NoUsefulMoves();
                    SaveSystem.Instance.RefreshSave();
                    return true;
                }
                return false;
            }
        }

        private static ECellState ThreeInARow
        {
            get
            {
                List<CellData> cells = TableController.Instance.Cells;

                for (int i = 0; i < 3; i++)
                {
                    int idHorz = i * 3 + 1;
                    ECellState checkingStateHorz = cells[idHorz].CellState;

                    if (cells[idHorz - 1].CellState == checkingStateHorz 
                        && cells[idHorz + 1].CellState == checkingStateHorz)
                    {
                        return checkingStateHorz;
                    }

                    int idVert = 3 + i;
                    ECellState checkingStateVert = cells[idVert].CellState;

                    if (cells[idVert - 3].CellState == checkingStateVert
                        && cells[idVert + 3].CellState == checkingStateVert)
                    {
                        return checkingStateVert;
                    }
                }

                ECellState checkingDiagonals = cells[4].CellState;
                if ((cells[0].CellState == checkingDiagonals && cells[8].CellState == checkingDiagonals)
                    || (cells[2].CellState == checkingDiagonals && cells[6].CellState == checkingDiagonals))
                {
                    return checkingDiagonals;
                }

                return ECellState.Empty;
            }
        }

        private static bool NoEmptyCells
        {
            get
            {
                List<CellData> cells = TableController.Instance.Cells;

                for (int i = 0; i < 3; i++)
                {
                    int idHorz = i * 3 + 1;

                    ECellState[] horzStates = new ECellState[] 
                    {
                        cells[idHorz - 1].CellState,
                        cells[idHorz].CellState,
                        cells[idHorz + 1].CellState
                    };

                    int idVert = 3 + i;

                    ECellState[] vertStates = new ECellState[]
                    {
                        cells[idVert - 3].CellState,
                        cells[idVert].CellState,
                        cells[idVert + 3].CellState
                    };

                    if (!LineIsBroken(horzStates) || !LineIsBroken(vertStates))
                    {
                        return false;
                    }
                }

                ECellState[] diag1States = new ECellState[]
                {
                        cells[0].CellState,
                        cells[4].CellState,
                        cells[8].CellState
                };

                ECellState[] diag2States = new ECellState[]
                {
                        cells[2].CellState,
                        cells[4].CellState,
                        cells[6].CellState
                };

                if (!LineIsBroken(diag1States) || !LineIsBroken(diag2States))
                {
                    return false;
                }

                return true;
            }
        }

        private static bool LineIsBroken(ECellState[] states)
        {
            bool isCircles = states.Contains(ECellState.Circle);
            bool isCrosses = states.Contains(ECellState.Cross);
            if (isCircles == isCrosses)
            {
                return isCircles;
            }
            else
            {
                return false;
            }
        }
    }
}