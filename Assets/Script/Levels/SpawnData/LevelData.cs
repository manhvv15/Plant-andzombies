using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Levels.SpawnData
{
    [Serializable]
    public class LevelData
    {
        public int plantLimit = 9;
        public int startingSun = 50;
        public bool noSunDrops = false;
        public string background;
        public List<WaveSpawnData> waves;
        public LevelCompletionInfo onComplete;
        public bool denySunProductionPlants;
        public bool invisiGhoul;
        public bool removeMowers;
    }
}
