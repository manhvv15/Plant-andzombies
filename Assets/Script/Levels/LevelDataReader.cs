using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Levels.SpawnData;
using UnityEngine;

namespace Assets.Script.Levels
{
    public class LevelDataReader : MonoBehaviour
    {
        public LevelData GetLevelData(string levelJsonPath)
        {
            TextAsset jsonFile = Resources.Load<TextAsset>(levelJsonPath);
            if (jsonFile)
            {
                throw new Exception("Level Data does not exists!");
            }
            string jsonString = jsonFile.text;
            LevelData levelData = JsonUtility.FromJson<LevelData>(jsonString);
            return levelData;
        }
    }
}
