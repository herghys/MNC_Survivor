using System.Collections;

using HerghysStudio.Survivor.Character;
using HerghysStudio.Survivor.Spawner;
using HerghysStudio.Survivor.Utility.Singletons;
using HerghysStudio.Survivor.WorldGeneration;

using UnityEngine;

namespace HerghysStudio.Survivor
{
    public class GameManager : NonPersistentSingleton<GameManager>
    {

        [Header("Data")]
        public PlayableCharacterData PlayerData;
        public MapData MapData;


        [Header("References")]
        [SerializeField] GameTimeManager TimerManager;
        [SerializeField] private WorldGenerator worldGenerator;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerSpawner playerSpawner;
        [SerializeField] private EnemySpawner enemySpawner;
        public PlayerController Player { get; private set; }


        public bool IsPaused { get; set; }

        public override void DoOnAwake()
        {
            base.DoOnAwake();
            worldGenerator ??= FindFirstObjectByType<WorldGenerator>();
            cameraController ??= FindFirstObjectByType<CameraController>();
            TimerManager ??= FindFirstObjectByType<GameTimeManager>();
            playerSpawner ??=FindFirstObjectByType<PlayerSpawner>();
            enemySpawner ??= FindFirstObjectByType<EnemySpawner>();

            playerSpawner?.SetupPlayerData(PlayerData);
            enemySpawner.Setup(MapData.Enemies);
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        IEnumerator Start()
        {
            Player = playerSpawner.SpawnPlayer();
            worldGenerator.SetPlayer(Player.transform);
            cameraController.SetupPlayer(Player.transform);
            enemySpawner.Setup(Player.transform);
            yield return null;
        }

        public void OnStartCountdownEnded()
        {

        }

        public void OnTimerEnded()
        {

        }

        public void OnPlayerDead()
        {

        }
    }
}