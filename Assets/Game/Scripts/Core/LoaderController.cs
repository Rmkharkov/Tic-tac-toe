namespace Game.Core
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LoaderController : MonoBehaviour
    {
        private void Awake()
        {
            SaveSystem.Instance.Load();
        }

        void Start()
        {
            SceneManager.LoadScene(1);
        }
    }
}