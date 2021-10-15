namespace Game.Configs
{
    using System.Collections.Generic;
    using UnityEngine;
    using Game.AI;

    [CreateAssetMenu(fileName = "AICharactersLoaderConfig", menuName = "Configs/AICharactersLoaderConfig")]
    public class AICharactersLoaderConfig : ScriptableObject
    {
        private static AICharactersLoaderConfig _instance;
        public static AICharactersLoaderConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<AICharactersLoaderConfig>("AICharactersLoaderConfig");
                }
                return _instance;
            }
        }

        [System.Serializable]
        public class AICharacterData
        {
            public EAIType AIType;
            public AICharacterBase CharacterSource;
        }

        [SerializeField]
        private List<AICharacterData>   _characters = new List<AICharacterData>();

        public AICharacterBase GetRandomAI
        {
            get
            {
                int id = UnityEngine.Random.Range(0, _characters.Count);
                return _characters[id].CharacterSource;
            }
        }
    }
}