using System.Collections;

using HerghysStudio.Survivor.Character;
using HerghysStudio.Survivor.Spawner;
using HerghysStudio.Survivor.Utility.Coroutines;
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
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField] public Transform VFXHolder { get; private set; }


        public PlayerController Player { get; private set; }

        public UnityAction OnTimerEnded;
        public UnityAction<EndGameState> OnGameEnded;
        public UnityAction<bool> OnTogglePause;
        public UnityAction OnClickedHome;
        public UnityAction OnGameStart;
        public int ActiveEnemies { get;  set; }
        public int KilledEnemies { get; set; }
        public long GoldCount { get;  set; }


        public bool IsHomeClicked { get; set; }
        public bool IsPaused { get; set; }
        public bool IsPlayerDead { get; private set; }
        public bool IsGameEnded { get; private set; }

        public override void DoOnAwake()
        {
            IsHomeClicked = false;
            IsPaused = false;
            IsPlayerDead = false;

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
            uiManager.UpdateCoin(GoldCount);
            uiManager.UpdateEnemyActiveCounter(ActiveEnemies);
            uiManager.UpdateEnemyKilled(KilledEnemies);

        }

        public void OnStartCountdownEnded()
        {
            OnGameStart?.Invoke();
            timerManager.StartLoop();
            enemySpawner.StartSpawning();

        }

        public void TogglePauseGame()
        {
            IsPaused = !IsPaused;
            OnTogglePause?.Invoke(IsPaused);
            uiManager.PauseGame(IsPaused);
        }

        public void MainMenu()
        {
            if (IsHomeClicked)
                return;

            OnClickedHome?.Invoke();
            IEMainMenu().Run();
        }

        private IEnumerator IEMainMenu()
        {
            IsHomeClicked = true;
            yield return new WaitForSeconds(0.25f);
            SceneManager.LoadScene("MainMenu");
        }

        public void WinGame()
        {
            EndGame(EndGameState.Win);
            uiManager.WinGame();
        }

        public void PlayerDead()
        {
            EndGame(EndGameState.Lose);
            IsPlayerDead = true;
            uiManager.LoseGame();
        }

        private void EndGame(EndGameState state)
        {
            IsGameEnded = true;
            OnGameEnded?.Invoke(state);
        }

        [ContextMenu("Start Game")]
        public void StartGame()
        {
            timerManager.StartGameTimer();
        }
    }

    public enum EndGameState
    {
        Win,
        Lose
    }
}
