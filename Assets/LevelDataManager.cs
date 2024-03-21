using Assets.Script.Levels.SpawnData;
using UnityEngine;

namespace Assets
{
    public class LevelDataManager : MonoBehaviour
    {
        public static LevelDataManager Instance;
        private string levelPath;
        private LevelData levelData;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SetLevelPath("Levels/test_level");
        }

        public string GetLevelPath() => levelPath;

        public void SetLevelPath(string newLevelPath)
        {
            levelPath = newLevelPath;
            levelData = GenerateLevelData();
        }

        public LevelData GetLevelData() { return levelData; }
        private LevelData GenerateLevelData()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>(levelPath);
            if (jsonFile == null)
            {
                Debug.LogError("Failed to load JSON file.");
                return null;
            }
            string jsonString = jsonFile.text;
            LevelData readLevelData = JsonUtility.FromJson<LevelData>(jsonString);
            if (readLevelData == null)
            {
                Debug.LogError("levelData has been read but it's null");
            }
            return readLevelData;
        }
    }
}
