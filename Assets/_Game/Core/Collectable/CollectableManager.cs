using HerghysStudio.Survivor.Utility.Singletons;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.Collectables
{
    public class CollectableManager : NonPersistentSingleton<CollectableManager>
    {
        [SerializeField] private GoldDrop prefab;
        [SerializeField] private HealthPickup health;
        [SerializeField] private Transform holder;
        public ObjectPool<Collectable> GoldDropPool { get; private set; }
        public ObjectPool<Collectable> HealthDropPool { get; private set; }

        public override void DoOnAwake()
        {
            base.DoOnAwake();
            GoldDropPool = new(CreateGold, GetGold, ReleaseGold, DestroyGold);
            HealthDropPool = new(CreateHealth, GetHealth, ReleaseHealth, DestroyHealth);
        }

        #region Gold
        private Collectable CreateGold()
        {
            var gold = Instantiate(prefab.gameObject, holder);
            gold.gameObject.SetActive(false);
            return gold.GetComponent<GoldDrop>();
        }

        private void GetGold(Collectable drop)
        {
            drop.gameObject.SetActive(true);
        }

        private void ReleaseGold(Collectable drop)
        {
            drop.gameObject.SetActive(false);
        }

        private void DestroyGold(Collectable drop)
        {
            DestroyGold(drop);
        }
        #endregion

        #region Health
        private Collectable CreateHealth()
        {
            var health = Instantiate(prefab.gameObject, holder);
            health.gameObject.SetActive(false);
            return health.GetComponent<GoldDrop>();
        }

        private void GetHealth(Collectable drop)
        {
            drop.gameObject.SetActive(true);
        }

        private void ReleaseHealth(Collectable drop)
        {
            drop.gameObject.SetActive(false);
        }

        private void DestroyHealth(Collectable drop)
        {
            DestroyGold(drop);
        }
        #endregion
    }
}
