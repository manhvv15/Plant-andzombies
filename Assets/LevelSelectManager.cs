using System.Collections.Generic;
using System.Linq;
using Assets.Script.Save;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets
{
    public class LevelSelectManager : MonoBehaviour
    {
        [SerializeField] private GameObject levelList;
        // Start is called before the first frame update
        void Start()
        {
            SaveGameData data = SaveGameManager.Instance.LoadGame();
            var children = levelList.GetComponentsInChildren<LevelSelectButton>();
            foreach (var child in children)
            {
                var button = child.GetComponent<Button>();
                if (child.alwaysUnlocked
                    || data.IsLevelCompleted(child.levelId))
                {
                    button.interactable = true;
                }
            }
        }

        public void UnlockAll()
        {
            SaveGameData data = SaveGameManager.Instance.LoadGame();
            data.UnlockedLevels = Enumerable.Range(0, 50).ToList();
            SaveGameManager.Instance.SaveGame(data);
            SceneManager.LoadScene("LevelSelectScreen");
        }
    
    }
}
