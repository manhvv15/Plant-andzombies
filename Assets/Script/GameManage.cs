using System.Security.Cryptography;
using Assets.Script.Levels.SpawnData;
using Assets.Script.Save;
using Assets.Script.Sound;
using Assets.Script.Zombies;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script
{
    public class GameManage : MonoBehaviour
    {
        public GameObject currentPlant;
        public Sprite currentPlantSprite;
        public Transform tiles;
        public LayerMask tileMask;
        public int suns;
        public TextMeshProUGUI sunText;
        public LayerMask sunMask;
        public bool isUsingShovel = false;

        private int plantCost = 0;
        private PlantScript plantScript;

        [SerializeField] private AudioClip _sunCollectClip;
        [SerializeField] private AudioClip _levelMusic;
        [SerializeField] private AudioClip _levelHordeMusic;
        [SerializeField] private AudioClip _levelWinClip;
        [SerializeField] private AudioClip _plantAudioClip;
        [SerializeField] private AudioClip _levelLoseClip;
        [SerializeField] private AudioClip _eatAudioClip;
        [SerializeField] private AudioClip _screamClip;
        [SerializeField] private SpriteRenderer backgroundRenderer;
        [SerializeField] private Canvas canvas;

        [SerializeField] private Transitioner transitioner;

        //This is for dynamically load plant in
        //TODO Load plants dynamically in from select screen
        [SerializeField] private GameObject[] plantSlots;
        [SerializeField] private GameObject[] plantSelected;

        [SerializeField] private PauseScreen pauseScreen;
        private PauseScreen activePauseScreen = null;
        private bool isPaused = false;
        public void BuyPlant(GameObject plant, Sprite sprite, int cost, PlantScript plantScript)
        {
            isUsingShovel = false;
            currentPlant = plant;
            currentPlantSprite = sprite;
            plantCost = cost;
            this.plantScript = plantScript;
        }

        private void Start()
        {
            GameStateManager.Instance.AllowPausing();
            GameStateManager.Instance.GamePaused.AddListener(GamePaused);
            GameStateManager.Instance.GameUnPaused.AddListener(GameResumed);
            LevelData levelData = LevelDataManager.Instance.GetLevelData();
            suns = levelData.startingSun;
            //Background
            if (levelData.background is { Length: > 0 })
            {
                var sprite = Resources.Load<Sprite>(levelData.background);
                if (sprite == null)
                {
                    Debug.LogError($"Failed to load background. ({levelData.background})");
                }
                else
                {
                    backgroundRenderer.sprite = sprite;
                }
            
            }
            //InvisiGhoul
            ZombieFactory.Instance.InvisiGhoulMode = levelData.invisiGhoul;
            //Remove all mowers
            if (levelData.removeMowers)
            {
                var mowers = GameObject.FindObjectsByType<Mower>(FindObjectsSortMode.None);
                foreach (var mower in mowers)
                {
                    Destroy(mower.gameObject);
                }
            }

            var selections = PlantSelectDataHandler.Instance.PlantSelections;
            if (selections == null || selections.Length == 0)
            {
                Debug.Log("Empty, loading debug plants.");
            }
            else
            {
                plantSelected = selections;
            }
        
            Debug.Log($"Selected {plantSelected.Length} plants.");
            int i = 0;
            foreach (var plant in plantSelected)
            {
                if (i < plantSlots.Length)
                {
                    var slot = plantSlots[i];
                    Instantiate(plant, slot.transform);
                    i++;
                }
                else
                {
                    break;
                }
            }
            SoundManager.Instance.PlayMusic(_levelMusic);
        }

        private void Update()
        {
            if (isPaused)
            {
                return;
            }
            sunText.text = suns.ToString();

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, tileMask);
    
            foreach(Transform t in tiles)
            {
                t.GetComponent<SpriteRenderer>().enabled = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelBuyPlant();
                isUsingShovel = false;
                return;
            }

            if (hit.collider )
            {
                var tile = hit.collider.GetComponent<Tile>();
                if (isUsingShovel && tile.plant != null && Input.GetMouseButtonDown(0))
                {
                    Destroy(tile.plant);
                    SoundManager.Instance.PlaySound(_plantAudioClip);
                    isUsingShovel = false;
                }
                else if (currentPlant && tile.plant == null)
                {
                    hit.collider.GetComponent<SpriteRenderer>().sprite = currentPlantSprite;
                    hit.collider.GetComponent<SpriteRenderer>().enabled = true;

                    if (Input.GetMouseButtonDown(0))
                    {
                        var plant = Instantiate(currentPlant, tile.transform.position, Quaternion.identity);
                        tile.plant = plant;
                        this.suns -= plantCost;
                        SoundManager.Instance.PlaySound(_plantAudioClip);
                        plantScript.Bought();
                        CancelBuyPlant();
                    }
                }
            }


            RaycastHit2D sunHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, sunMask);

            if (sunHit.collider)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var sun = sunHit.collider.GetComponent<Sun>();
                    suns += sun.GetAmount();
                    Destroy(sunHit.collider.gameObject);
                    SoundManager.Instance.PlaySound(_sunCollectClip);
                }
            }

        }

        private void CancelBuyPlant()
        {
            plantCost = 0;
            currentPlant = null;
            currentPlantSprite = null;
        }

        public void GetShovel()
        {
            CancelBuyPlant();
            isUsingShovel = true;
        }

        public void Win()
        {
            Debug.Log("You win!");
            transitioner.FadeWhite(5);
            //TODO: Replace this with an object to click on to perform the action below.
            SoundManager.Instance.StopMusic();
            SoundManager.Instance.PlaySound(_levelWinClip);
            Invoke(nameof(AfterWin),7);
        }

        private void AfterWin()
        {
            var levelData = LevelDataManager.Instance.GetLevelData();
            var onComplete = levelData.onComplete;
            if (onComplete != null)
            {
                var saveGameData = SaveGameManager.Instance.LoadGame();
                var success = saveGameData.AddCompletedLevel(onComplete.levelUnlockId);
                Debug.Log($"Add level is {success}");
                SaveGameManager.Instance.SaveGame(saveGameData);
                Debug.Log("Saved.");
            }
            else
            {
                Debug.LogWarning("No onComplete property.");
            }
            GameStateManager.Instance.DenyPausing();
            Exit();
        }

        public void TurnOnHordeMusic()
        {
            SoundManager.Instance.SwapMusic(_levelHordeMusic);
        }

        private void GamePaused()
        {
            if (activePauseScreen)
            {
                Destroy(activePauseScreen.gameObject);
            }
            isPaused = true;
            activePauseScreen = Instantiate(pauseScreen, canvas.transform);
        }

        private void GameResumed()
        {
            if (activePauseScreen)
            {
                Destroy(activePauseScreen.gameObject);
            }
            isPaused = false;
        }

        public void Lose()
        {
            GameStateManager.Instance.DenyPausing();
            SoundManager.Instance.StopMusic();
            SoundManager.Instance.PlaySound(_levelLoseClip);
            transitioner.FadeBlack(0);
            transitioner.ShowLoseImage(6);
            Invoke(nameof(No),6);
            Invoke(nameof(EatBrain),5);
        }

        private void EatBrain()
        {
            SoundManager.Instance.PlaySound(_eatAudioClip);
        }

        private void No()
        {
            SoundManager.Instance.PlaySound(_screamClip);
            Invoke(nameof(Exit),5);
        }

        private void Exit()
        {
            SceneManager.LoadScene("StartScreen");
        }

    }
}
