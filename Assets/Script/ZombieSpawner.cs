using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class ZombieSpawner : MonoBehaviour
    {
        public Transform[] spwanPoints;

        public GameObject Zombie;

        public ZombieTypeProb[] zombieTypes;

        private List<ZombieType> probList = new List<ZombieType>(); 

        private void Start()
        {
            InvokeRepeating("SpawnZombie", 2, 5);

            foreach (ZombieTypeProb zom in zombieTypes)
            {
                for (int i =0; i < zom.probability; i++)
                {
                    probList.Add(zom.type);
                }
            }
        }
        void SpawnZombie()
        {
            int r = Random.Range(0, spwanPoints.Length); 
            GameObject myZombie = Instantiate(Zombie, spwanPoints[r].position, Quaternion.identity);
            myZombie.GetComponent<Zombie>().type = probList[Random.Range(0, probList.Count)];
        }
    }

    [System.Serializable]
    public class ZombieTypeProb
    {
        public ZombieType type;

        public int probability;
    }
}