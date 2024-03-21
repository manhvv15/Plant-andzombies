using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Script.Save
{
    public class SaveGameManager : MonoBehaviour
    {
        public static SaveGameManager Instance;
        void Awake()
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

        public SaveGameData LoadGame()
        {
            string destination = Application.persistentDataPath + "/save.dat";
            FileStream file;
            SaveGameData data;
            if (File.Exists(destination))
            {
                try
                {
                    file = File.OpenRead(destination);
                    BinaryFormatter bf = new BinaryFormatter();
                    data = (SaveGameData)bf.Deserialize(file);
                    file.Close();
                }
                catch (SerializationException)
                {
                    data = OverrideNew();
                }
            }
            else
            {
                data = OverrideNew();
            }
            return data;
        }

        public SaveGameData OverrideNew()
        {
            SaveGameData data = new SaveGameData()
            {
                UnlockedLevels = new List<int>() { 0 },
            };
            SaveGame(data);
            return data;
        }

        public void SaveGame(SaveGameData data)
        {
            string destination = Application.persistentDataPath + "/save.dat";
            FileStream file;

            if (File.Exists(destination)) file = File.OpenWrite(destination);
            else file = File.Create(destination);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file,data);
            file.Close();
        }
    }
}
