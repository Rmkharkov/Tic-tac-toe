namespace Game.AI
{
    using System.Collections.Generic;
    using UnityEngine;
    using Game.Gameplay;
    using Game.Cells;
    using Game.UI;
    using System.Linq;

    public interface IAIController
    {
        void CanMove();
    }

    public class AIController : MonoBehaviour, IAIController
    {
        private static IAIController _instance;
        public static IAIController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AIController();
                }
                return _instance;
            }
        }

        private List<CellData>  _cells = new List<CellData>();
        private float           _delayWaitTimer;

#region IAIController

        public void CanMove()
        {
            UIView.Current.ShowAIWait(delegate { DoMoveOnCell(); });
        }

#endregion

        void DoMoveOnCell()
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
                    moveCell = _cells
                    .Where(c => c.CellState == ECellState.Empty)
                    .First();
                }
            }

            moveCell.CellItem.CellButton.onClick.Invoke();
        }


        private CellData TwoCellsInRow
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
                        _cells[idsV[0]].CellState,
                        _cells[idsV[0]].CellState
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

        private int LineNeedsStep(ECellState[] states)
        {
            bool isCircles = states.Contains(ECellState.Circle);
            bool isCrosses = states.Contains(ECellState.Cross);
            if (isCircles != isCrosses)
            {
                int total = 0;
                int emptyId = -1;
                for(int i = 0; i < states.Length; i++)
                {
                    if (states[i] != ECellState.Empty)
                    {
                        total++;
                    }
                    else
                    {
                        emptyId = i;
                    }
                }

                if (total == 2)
                {
                    return emptyId;
                }
            }

            return -1;
        }
    }
}