namespace Game.Gameplay
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface ITableView
    {
        Transform CellsParent { get; }
    }

    public class TableView : MonoBehaviour, ITableView
    {
        [SerializeField]
        private Transform       _cellsParent;

        public Transform CellsParent => _cellsParent;
    }
}