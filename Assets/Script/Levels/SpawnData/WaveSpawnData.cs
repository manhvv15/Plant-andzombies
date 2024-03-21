using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Levels.SpawnData
{
    [System.Serializable]
    public class WaveSpawnData
    {
        public List<ZombieSpawnData> zombies;
        public bool isLargeWave;
        public bool spreadEvenly;
    }
}
