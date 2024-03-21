using System;

namespace Assets.Script.Levels.SpawnData
{
    [System.Serializable]
    public class ZombieSpawnData
    {
        public ZombieSpawnData()
        {
        }

        public string type;
        public int amount;
        public string row;
    }
}
