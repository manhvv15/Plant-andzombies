using UnityEngine;

namespace Assets.Script
{
    public class SunSpawner : MonoBehaviour
    {
        public GameObject sunObject;

        private void Start()
        {
            var levelData = LevelDataManager.Instance.GetLevelData();
            if (!levelData.noSunDrops)
            {
                SpawnSun();
            }
        }
        void SpawnSun()
        {
            GameObject mySun = Instantiate(sunObject, new Vector3(Random.Range(-4f, 8.35f), 6, 0), Quaternion.identity);
            mySun.GetComponent<Sun>().dropTpYPos = Random.Range(2f, -3f); 
            Invoke("SpawnSun", Random.Range(7, 9));
        }
    }
}
