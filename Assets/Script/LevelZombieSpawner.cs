using System.Collections.Generic;
using Assets.Script.Levels.SpawnData;
using Assets.Script.Sound;
using Assets.Script.Zombies;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Assets.Script
{
    public class LevelZombieSpawner : MonoBehaviour
    {
        [CanBeNull] public LevelData LevelData = null;
        public Transform[] SpawnPoints;
        public GameObject Zombie;

        [SerializeField] private AudioClip _firstWaveClip;
        [SerializeField] private AudioClip _nextWaveClip;
        [SerializeField] private AudioClip _largeWaveClip;
        [SerializeField] private AudioClip _nextLargeWaveClip;
        [SerializeField] private AudioClip _finalWaveClip;
        [SerializeField] private WarningText _warningText;
        [SerializeField] private List<ZombieType> _zombieTypes;
        [SerializeField] private GameManage manager;
        [SerializeField] private LevelProgressBar progressBar;

        private WaveSpawnData tempData;

        private readonly Dictionary<string, int> _zombieIndexes = new()
        {
            {"normal",0},
            {"conehead",1},
            {"buckethead",2}
        };

        /// <summary>
        /// How long until the first wave starts
        /// </summary>
        public int GracePeriod = 20;
        /// <summary>
        /// How long since the previous wave until the game starts to check whether to begin the next wave.
        /// </summary>
        public int WaveGracePeriod = 10;
        /// <summary>
        /// Maximum time a wave can last before the next wave spawn.
        /// </summary>
        public int MaximumWavePeriod = 30;
        /// <summary>
        /// Tracks what waves this is
        /// </summary>
        private int _currentWave = 0;
        /// <summary>
        /// Tracks how long this wave has been going.
        /// </summary>
        private int _waveTimer = 0;
        private bool isFinalWave = false;

        // Start is called before the first frame update
        void Start()
        {
            LevelData = LevelDataManager.Instance.GetLevelData();
            Debug.Log($"Wave Count: {LevelData.waves.Count}");
            progressBar.SetMax(LevelData.waves.Count - 1);
            Invoke(nameof(BeginSpawning),GracePeriod);
        }

        void BeginSpawning()
        {
            progressBar.gameObject.SetActive(true);
            Debug.Log("The zombies are coming.");
            SoundManager.Instance.PlaySound(_firstWaveClip);
            StartNextWave();
        }

        void StartNextWave()
        {
            Debug.Log($"Attempting to start wave {_currentWave}");
            if (_currentWave == LevelData.waves.Count)
            {
                TryWin();
                return;
            } else if (_currentWave + 1 == LevelData.waves.Count)
            {
                Debug.Log("Final Wave");
                isFinalWave = true;
            }
            progressBar.SetValue(_currentWave);
            var waveData = LevelData.waves[_currentWave];
            if (waveData.isLargeWave)
            {
                tempData = waveData;
                Debug.Log("A huge wave of zombie is approaching");
                _warningText.ShowText(WarningText.HUGE_WAVE,5);
                SoundManager.Instance.PlaySound(_largeWaveClip);
                Invoke(nameof(SpawnWaveAsLargeWave),7);
                Invoke(nameof(TurnOnHordeMusic),5);
            }
            else
            {
                SoundManager.Instance.PlaySound(_nextWaveClip);
                SpawnWave(waveData);
            }
        
        }

        private void TurnOnHordeMusic()
        {
            Debug.Log("Turned on Horde Music.");
            manager.TurnOnHordeMusic();
        }

        private void SpawnWave(WaveSpawnData waveData)
        {
            SpawnZombies(waveData);
            _waveTimer = WaveGracePeriod;
            _currentWave++;
            InvokeRepeating(nameof(CheckNextSpawn), WaveGracePeriod, 1);
            if (isFinalWave)
            {
                SoundManager.Instance.PlaySound(_finalWaveClip);
                _warningText.ShowText(WarningText.FINAL_WAVE);
            }
        }

        private void SpawnWaveAsLargeWave()
        {
            //TODO Spawn Flag Zombie
            SoundManager.Instance.PlaySound(_nextLargeWaveClip);
            SpawnWave(tempData);
        }

        private void TryWin()
        {
            if (GameObject.FindObjectsOfType(typeof(Zombie)).Length == 0)
            {
                Win();
            }
            else
            {
                Invoke(nameof(TryWin),1);
            }
        }

        private void SpawnZombies(WaveSpawnData waveSpawnData)
        {
            var zombies = waveSpawnData.zombies;
            if (waveSpawnData.spreadEvenly)
            {
                SpawnZombiesEvenly(zombies);
            }
            else
            {
                SpawnZombiesAnywhere(zombies);
            }
            //foreach (var zombie in zombies)
            //{
            //    int r = UnityEngine.Random.Range(0, SpawnPoints.Length);
            //    for (int i = 0; i < zombie.amount; i++)
            //    {
            //        GameObject myZombie = Instantiate(Zombie, SpawnPoints[r].position, Quaternion.identity);
            //    }
            //}
        
        }

        private void SpawnZombiesEvenly(List<ZombieSpawnData> zombies)
        {
            List<int> spawnPositions = new () { 0, 1, 2, 3, 4 };
            foreach (var zombie in zombies)
            {
                for (int i = 0; i < zombie.amount; i++)
                {
                    if (spawnPositions.Count == 0)
                    {
                        spawnPositions = new List<int>() { 0, 1, 2, 3, 4 };
                    }
                    var r = Random.Range(0, spawnPositions.Count);
                    var randomSpawnPos = spawnPositions[r];
                    var spawnPosition = GetSpawnPosition(SpawnPoints[randomSpawnPos].position);
                    spawnPositions.RemoveAt(r);
                    ZombieFactory.Instance.InstantiateFromType(spawnPosition, zombie.type);
                    //if (TryGetType(zombie.type,out var type))
                    //{
                    //    //myZombie.GetComponent<Zombie>().type = type;
                    //}
                }
            }
        }

        private Vector3 GetSpawnPosition(Vector3 spawnPoint)
        {
            var spawnPosition = new Vector3(spawnPoint.x, spawnPoint.y, spawnPoint.z);
            spawnPosition.x += Random.Range(-1f, 1f);
            return spawnPosition;
        } 

        private bool TryGetType(string typeString, out ZombieType type)
        {
            if (_zombieIndexes.TryGetValue(typeString, out var index))
            {
                type = _zombieTypes[index];
                return true;
            }
            type = null;
            return false;
        }

        private void SpawnZombiesAnywhere(List<ZombieSpawnData> zombies)
        {
            foreach (var zombie in zombies)
            {
                for (int i = 0; i < zombie.amount; i++)
                {
                    var r = Random.Range(0, SpawnPoints.Length);
                    var spawnPosition = GetSpawnPosition(SpawnPoints[r].position);
                    ZombieFactory.Instance.InstantiateFromType(spawnPosition, zombie.type);
                    //if (TryGetType(zombie.type, out var type))
                    //{
                    //    //myZombie.GetComponent<Zombie>().type = type;
                    //}
                }
            }
        }

        private void CheckNextSpawn()
        {
            if (_waveTimer >= MaximumWavePeriod)
            {
                CancelInvoke(nameof(CheckNextSpawn));
                StartNextWave();
                Debug.Log("Timeout, began next wave.");
                return;
            }
            //Check if there are any zombies remaining on screen
            var remainingZombies = GetZombieCount();
            if (remainingZombies == 0)
            {
                CancelInvoke(nameof(CheckNextSpawn));
                StartNextWave();
                Debug.Log($"No Zombies Remaining, begin next wave.");
                return;
            }

            _waveTimer++;
        }

        private int GetZombieCount()
        {
            return GameObject.FindObjectsOfType(typeof(Zombie)).Length;
        }

        void Win()
        {
            manager.Win();
        }
    }
}
