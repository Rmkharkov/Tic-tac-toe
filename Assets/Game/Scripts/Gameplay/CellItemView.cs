namespace Game.Cells
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public interface ICellItemView
    {
        Button CellButton       { get; }
        GameObject Cross        { get; }
        GameObject Circle       { get; }
    }

    public class CellItemView : MonoBehaviour, ICellItemView
    {
        [SerializeField]
        private Button          _cellButton;
        [SerializeField]
        private GameObject      _cross;
        [SerializeField]
        private GameObject      _circle;

        public Button CellButton => _cellButton;

        public GameObject Cross => _cross;

        public GameObject Circle => _circle;
    }
}