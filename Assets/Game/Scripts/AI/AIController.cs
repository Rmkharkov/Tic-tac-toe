namespace Game.AI
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;
    using Game.Core;
    using Game.Configs;
    using Game.Cells;
    using Game.UI;
    using Game.Gameplay;
    using UniRx;

    public interface IAIController
    {
        void        Initialize();
        void        CanMove();
    }

    public class AIController : MonoBehaviour, IAIController, IDisposable
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

        private List<CellData>          _cells = new List<CellData>();
        private float                   _delayWaitTimer;
        private AICharacterBase         _currentAI;

        readonly CompositeDisposable    _lifetimeDisposables = new CompositeDisposable();

#region IAIController

        public void CanMove()
        {
            UIView.Current.ShowAIWait(delegate { DoMoveOnCell(); });
        }

        public void Initialize()
        {
            TableController.Instance.TableReset
                .Subscribe(_ => RepickAI())
                .AddTo(_lifetimeDisposables);

            RepickAI();
        }

#endregion

        void DoMoveOnCell()
        {
            CellData moveCell = _currentAI.CellMoveSolution;

            moveCell.CellItem.CellButton.onClick.Invoke();
        }

        private void RepickAI()
        {
            _currentAI = AICharactersLoaderConfig.Instance.GetRandomAI;
        }

        public void Dispose() => _lifetimeDisposables.Clear();
    }
}