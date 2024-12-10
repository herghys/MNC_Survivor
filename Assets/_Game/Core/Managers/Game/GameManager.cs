using System.Collections;

using HerghysStudio.Survivor.Character;
using HerghysStudio.Survivor.Spawner;
using HerghysStudio.Survivor.Utility.Singletons;
using HerghysStudio.Survivor.WorldGeneration;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace HerghysStudio.Survivor
{
    public class GameManager : NonPersistentSingleton<GameManager>
    {

        [Header("Data")]
        public PlayableCharacterData PlayerData;
        public MapData MapData;


        [Header("References")]
        [SerializeField] private GameUIManager uiManager;
        [SerializeField] private GameTimeManager timerManager;
        [SerializeField] private WorldGenerator worldGenerator;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerSpawner playerSpawner;
        [SerializeField] private EnemySpawner enemySpawner;
        public PlayerController Player { get; private set; }

        public UnityAction OnPlayerDead;
        public int ActiveEnemies { get; set; }
        public int KilledEnemies { get; set; }

        public bool IsPaused { get; set; }
        public bool IsPlayerDead { get; private set; }

        public override void DoOnAwake()
        {
            base.DoOnAwake();
            worldGenerator ??= FindFirstObjectByType<WorldGenerator>();
            cameraController ??= FindFirstObjectByType<CameraController>();
            timerManager ??= FindFirstObjectByType<GameTimeManager>();
            playerSpawner ??=FindFirstObjectByType<PlayerSpawner>();
            enemySpawner ??= FindFirstObjectByType<EnemySpawner>();
            uiManager ??= FindFirstObjectByType<GameUIManager>();

            playerSpawner?.SetupPlayerData(PlayerData);
            enemySpawner?.Setup(MapData.Enemies);

            Player = playerSpawner.SpawnPlayer();
            worldGenerator.SetPlayer(Player.transform);
            cameraController.SetupPlayer(Player.transform);
            enemySpawner?.SetupPlayerAndPool(Player.transform);
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartGame();
        }

        private void LateUpdate()
        {
            uiManager.UpdateEnemyKilled(KilledEnemies);
            uiManager.UpdateEnemyActiveCounter(ActiveEnemies);
        }

        public void OnStartCountdownEnded()
        {
            timerManager.StartLoop();
            enemySpawner.StartSpawning();

        }

        public void TogglePauseGame()
        {
            IsPaused = !IsPaused;
            uiManager.PauseGame(IsPaused);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void OnTimerEnded()
        {

        }

        public void PlayerDead()
        {
            IsPlayerDead = true;
            OnPlayerDead?.Invoke();
        }

        [ContextMenu("Start Game")]
        public void StartGame()
        {
            timerManager.StartGameTimer();
        }
    }
}
