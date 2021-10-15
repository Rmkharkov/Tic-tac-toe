namespace Game.Configs
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Game.Prefabs;
    using System.Linq;

    [System.Serializable]
    public class PrefabData
    {
        public EGameplayPrefab  PrefabType;
        public GameObject       PrefabObject;
    }

    [CreateAssetMenu(fileName = "PrefabsLoaderConfig", menuName = "Configs/PrefabsLoaderConfig")]
    public class PrefabsLoaderConfig : ScriptableObject
    {
        private static PrefabsLoaderConfig _instance;
        public static PrefabsLoaderConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<PrefabsLoaderConfig>("PrefabsLoaderConfig");
                }
                return _instance;
            }
        }

        [SerializeField]
        private List<PrefabData> GameplayPrefabs = new List<PrefabData>();

        public GameObject GetPrefab(EGameplayPrefab prefabType)
        {
            return GameplayPrefabs
                .Where(c => c.PrefabType == prefabType)
                .First().PrefabObject;
        }
    }
}