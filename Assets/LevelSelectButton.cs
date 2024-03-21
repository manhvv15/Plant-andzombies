using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets
{
    public class LevelSelectButton : MonoBehaviour
    {
        [SerializeField] private string levelPath;
        [SerializeField] private string levelName;
        public bool alwaysUnlocked = false;
        public int levelId;
        [SerializeField] private TextMeshProUGUI levelNameText;
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(StartLevel);
        }

        private void StartLevel()
        {
            LevelDataManager.Instance.SetLevelPath(levelPath);
            SceneManager.LoadScene("PlantSelect");
        }

        private void OnValidate()
        {
            if (levelNameText != null)
            {
                levelNameText.text = levelName;
            }
        }
    }
}
