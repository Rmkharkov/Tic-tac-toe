namespace Game.AI
{
    using Game.Gameplay;
    using Game.Cells;
    using UnityEngine;
    using System.Linq;

    [CreateAssetMenu(fileName = "AIHard", menuName = "AICharacters/AIHard")]
    public class AICharacterHard : AICharacterBase
    {
        public override CellData CellMoveSolution
        {
            get
            {
                _cells = TableController.Instance.Cells;

                CellData moveCell = TwoCellsInRow;

                if (moveCell == null)
                {
                    if (_cells[4].CellState == ECellState.Empty)
                    {
                        moveCell = _cells[4];
                    }
                    else
                    {
                        var rnd = new System.Random();
                        moveCell = _cells
                        .Where(c => c.CellState == ECellState.Empty)
                        .OrderBy(r => rnd.Next())
                        .First();
                    }
                }

                return moveCell;
            }
        }

        public override CellData TwoCellsInRow
        {
            get
            {
                int to_returnId = -1;
                for (int i = 0; i < 3; i++)
                {
                    int[] idsH = {
                        i * 3,
                        i * 3 + 1,
                        i * 3 + 2
                    };

                    ECellState[] horzStates = new ECellState[]
                    {
                        _cells[idsH[0]].CellState,
                        _cells[idsH[1]].CellState,
                        _cells[idsH[2]].CellState
                    };

                    to_returnId = LineNeedsStep(horzStates);
                    if (to_returnId >= 0)
                    {
                        return _cells[idsH[to_returnId]];
                    }

                    int[] idsV =
                    {
                        i,
                        i + 3,
                        i + 6
                    };
                    ECellState[] vertStates = new ECellState[]
                    {
                        _cells[idsV[0]].CellState,
                        _cells[idsV[1]].CellState,
                        _cells[idsV[2]].CellState
                    };

                    to_returnId = LineNeedsStep(vertStates);
                    if (to_returnId >= 0)
                    {
                        return _cells[idsV[to_returnId]];
                    }
                }

                ECellState[] diag1States = new ECellState[]
                {
                        _cells[0].CellState,
                        _cells[4].CellState,
                        _cells[8].CellState
                };

                to_returnId = LineNeedsStep(diag1States);
                if (to_returnId >= 0)
                {
                    return _cells[to_returnId * 4];
                }

                ECellState[] diag2States = new ECellState[]
                {
                        _cells[2].CellState,
                        _cells[4].CellState,
                        _cells[6].CellState
                };

                to_returnId = LineNeedsStep(diag2States);
                if (to_returnId >= 0)
                {
                    return _cells[2 + to_returnId * 2];
                }

                return null;
            }
        }
    }
}