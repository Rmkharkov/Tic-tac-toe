namespace Game.Core
{
    using Game.Gameplay;
    using Game.UI;
    using Game.AI;
    using UnityEngine;

    public class ControllersInstaller : MonoBehaviour
    {
        private void Start()
        {
            TableController.Instance.Initialize();
            UIController.Instance.Initialize();
            AIController.Instance.Initialize();
        }
    }
}