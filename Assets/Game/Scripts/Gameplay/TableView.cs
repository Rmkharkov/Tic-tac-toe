namespace Game.Gameplay
{
    using UnityEngine;

    public interface ITableView
    {
        Transform CellsParent { get; }
    }

    public class TableView : MonoBehaviour, ITableView
    {
        public static ITableView Current;

        [SerializeField]
        private Transform       _cellsParent;

#region ITableView

        public Transform        CellsParent => _cellsParent;

#endregion

        private void Awake()
        {
            Current = this;
        }
    }
}