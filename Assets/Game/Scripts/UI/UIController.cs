namespace Game.UI
{
    using UnityEngine;
    using Game.Cells;
    using Game.Gameplay;

    public interface IUIController
    {
        void Initialize();
        void NoUsefulMoves();
        void GetWinner(ECellState winnerType);
        void ChangePlayer(ECellState nextPlayer);
    }

    public class UIController : MonoBehaviour, IUIController
    {
        private static IUIController _instance;
        public static IUIController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UIController();
                }
                return _instance;
            }
        }

        private IUIView         _uIView;
        private bool            _restartButtonSubscribed;

#region IUIController

        public void Initialize()
        {
            _uIView = UIView.Current;
            ChangePlayer(ECellState.Cross);
        }

        public void NoUsefulMoves()
        {
            _uIView.NoMovesScreen.SetActive(true);
            _uIView.RestartButton.gameObject.SetActive(true);
        }

        public void GetWinner(ECellState winnerType)
        {
            string winText = (winnerType == ECellState.Circle ? "Circles" : "Crosses") + " won!";
            _uIView.WinnerDescriptionText.text = winText;

            _uIView.WinnerScreen.SetActive(true);
            _uIView.RestartButton.gameObject.SetActive(true);
        }

        public void ChangePlayer(ECellState nextPlayer)
        {
            bool isNextCross = nextPlayer == ECellState.Cross;

            _uIView.CircleWait.SetActive(!isNextCross);
            _uIView.CrossWait.SetActive(isNextCross);
        }

#endregion
    }
}