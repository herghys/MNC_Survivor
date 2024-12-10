using HerghysStudio.Survivor.Utility.Singletons;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.Collectable
{
    public class CollectableManager : NonPersistentSingleton<CollectableManager>
    {
        [SerializeField] private GoldDrop prefab;
        [SerializeField] private Transform holder;
        public ObjectPool<GoldDrop> GoldDropPool { get; private set; }

        public override void DoOnAwake()
        {
            base.DoOnAwake();
            GoldDropPool = new(CreateGold, GetGold, ReleaseGold, DestroyGold);
        }

        private GoldDrop CreateGold()
        {
            var gold = Instantiate(prefab.gameObject, holder);
            gold.gameObject.SetActive(false);
            return gold.GetComponent<GoldDrop>();
        }

        private void GetGold(GoldDrop drop)
        {
            drop.gameObject.SetActive(true);
        }

        private void ReleaseGold(GoldDrop drop)
        {
            drop.gameObject.SetActive(false);
        }

        private void DestroyGold(GoldDrop drop)
        {
            DestroyGold(drop);
        }
    }
}
