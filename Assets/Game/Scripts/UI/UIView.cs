namespace Game.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using Game.Gameplay;

    public interface IUIView
    {
        GameObject NoMovesScreen { get; }
        GameObject WinnerScreen { get; }
        GameObject CrossWait { get; }
        GameObject CircleWait { get; }
        Button RestartButton { get; }
        Text WinnerDescriptionText { get; }
    }

    public class UIView : MonoBehaviour, IUIView
    {
        public static IUIView Current;

        [SerializeField]
        private GameObject      _noMovesScreen;

        [SerializeField]
        private GameObject      _winnerScreen;

        [SerializeField]
        private GameObject      _crossWait, _circleWait;

        [SerializeField]
        private Button          _restartButton;

        [SerializeField]
        private Text            _winnerDescriptionText;

#region IUIView

        public GameObject NoMovesScreen => _noMovesScreen;

        public GameObject WinnerScreen => _winnerScreen;

        public Text WinnerDescriptionText => _winnerDescriptionText;

        public Button RestartButton => _restartButton;

        public GameObject CrossWait => _crossWait;

        public GameObject CircleWait => _circleWait;

#endregion

        private void Awake()
        {
            Current = this;

            _restartButton.onClick.AddListener(delegate
            {
                WinnerScreen.SetActive(false);
                NoMovesScreen.SetActive(false);
                _restartButton.gameObject.SetActive(false);
                PickSellsLogic.RefreshTable();
            });
        }

    }
}