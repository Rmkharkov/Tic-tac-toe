namespace Game.AI
{
    using System.Collections.Generic;
    using UnityEngine;
    using Game.Gameplay;
    using Game.Cells;
    using System.Linq;

    public class AICharacterBase : ScriptableObject
    {
        [HideInInspector]
        public List<CellData>   _cells = new List<CellData>();

        public virtual CellData CellMoveSolution
        {
            get
            {
                _cells = TableController.Instance.Cells;
                return _cells[0];
            }
        }

        public virtual CellData TwoCellsInRow
        {
            get
            {
                return null;
            }
        }

        public int LineNeedsStep(ECellState[] states)
        {
            bool isCircles = states.Contains(ECellState.Circle);
            bool isCrosses = states.Contains(ECellState.Cross);
            if (isCircles != isCrosses)
            {
                int total = 0;
                int emptyId = -1;
                for (int i = 0; i < states.Length; i++)
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