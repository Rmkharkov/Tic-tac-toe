namespace Game.Gameplay
{
    using UnityEngine;

    public class TableView : MonoBehaviour
    {
        [SerializeField]
        private Transform       _cellsParent;

        public Transform CellsParent => _cellsParent;
    }
}