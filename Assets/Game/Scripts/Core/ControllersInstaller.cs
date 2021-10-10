namespace Game.Core
{
    using Game.Gameplay;
    using Game.UI;
    using UnityEngine;

    public class ControllersInstaller : MonoBehaviour
    {
        private void Start()
        {
            TableController.Instance.Initialize();
            UIController.Instance.Initialize();
        }
    }
}