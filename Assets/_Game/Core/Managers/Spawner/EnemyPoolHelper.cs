using System.Collections;
using System.Collections.Generic;
using HerghysStudio.Survivor.Character;
using HerghysStudio.Survivor.Spawner;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.Spawner
{
    public class EnemyPoolHelper : MonoBehaviour
    {
        public ObjectPool<EnemyController> enemyPools;

        public EnemyController prefab;
        [field: SerializeField] public EnemyCharacterData Data { get; private set; }
        [field: SerializeField] public Transform Holder { get; private set; }
        [field: SerializeField] public Transform Target { get; private set; }
        [field: SerializeField] public EnemySpawner Spawner { get; private set; }

        public void SetupContext(EnemySpawner spawner)
        {
            Spawner = spawner;
        }

        public void SetupData(EnemyCharacterData data, Transform holder)
        {
            Data = data;
            Holder = holder;
            prefab = Data.Prefab;
        }

        public void SetupTarget(Transform target)
        {
            Target = target;
        }

        [ContextMenu("Setup Pool")]
        public void SetupPool()
        {
            if (prefab == null || Holder == null || Target == null)
            {
                Debug.LogError("EnemyPoolManager is missing required setup data!");
                return;
            }

            enemyPools = new ObjectPool<EnemyController>(
                CreatePooledEnemy,
                GetPooledEnemy,
                ReleasePooledEnemy,
                DestroyPooledEnemy
            );

            Debug.Log("Enemy pool successfully set up!");
        }

        private EnemyController CreatePooledEnemy()
        {
            if (prefab == null || Holder == null)
            {
                Debug.LogError("Prefab or Holder is null. Cannot create pooled enemy.");
                return null;
            }

            var enemy = Instantiate(prefab);
            enemy.PoolReference = enemyPools;
            enemy.SetupData(Data);
            enemy.gameObject.SetActive(false);
            return enemy;
        }

        private void GetPooledEnemy(EnemyController enemy)
        {
            enemy.gameObject.SetActive(true);
            enemy.SetupData(Data);
            enemy.SetupTargetReference(Target);
        }

        private void ReleasePooledEnemy(EnemyController enemy)
        {
            enemy.ResetCharacter();
            enemy.gameObject.SetActive(false);
        }

        private void DestroyPooledEnemy(EnemyController enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}
