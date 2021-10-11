namespace Game.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using Game.Gameplay;
    using System;
    using System.Collections;

    public interface IUIView
    {
        GameObject NoMovesScreen { get; }
        GameObject WinnerScreen { get; }
        GameObject CrossWait { get; }
        GameObject CircleWait { get; }
        Button RestartButton { get; }
        Text WinnerDescriptionText { get; }
        void ShowAIWait(Action onEnd);
    }

    public class UIView : MonoBehaviour, IUIView
    {
        public static IUIView Current;

        [SerializeField]
        private GameObject      _noMovesScreen;

        [SerializeField]
        private GameObject      _winnerScreen;

        [SerializeField]
        private GameObject      _aiScreen;

        [SerializeField]
        private GameObject      _crossWait, _circleWait;

        [SerializeField]
        private Button          _restartButton;

        [SerializeField]
        private Text            _winnerDescriptionText;

        [SerializeField]
        private Text            _aiWaitText;

#region IUIView

        public GameObject NoMovesScreen =>      _noMovesScreen;

        public GameObject WinnerScreen =>       _winnerScreen;

        public Text WinnerDescriptionText =>    _winnerDescriptionText;

        public Button RestartButton =>          _restartButton;

        public GameObject CrossWait =>          _crossWait;

        public GameObject CircleWait =>         _circleWait;

        public void ShowAIWait(Action onEnd)
        {
            StartCoroutine(ShowAIThink(onEnd));
        }

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

        private IEnumerator ShowAIThink(Action onEnd)
        {
            _aiScreen.SetActive(true);
            float timer = 2f;
            int steps = 8;
            for (int i = 0; i < steps; i++)
            {
                int dotsCount = i % 4;
                _aiWaitText.text = "AI thinks" + DotsAdd(dotsCount);
                yield return new WaitForSeconds(timer / steps);
            }

            _aiScreen.SetActive(false);
            if (onEnd != null)
            {
                onEnd.Invoke();
            }
            yield break;
        }

        string DotsAdd(int count)
        {
            string to_return = "";
            for (int i = 0; i < count; i++)
            {
                to_return += ".";
            }

            return to_return;
        }
    }
}