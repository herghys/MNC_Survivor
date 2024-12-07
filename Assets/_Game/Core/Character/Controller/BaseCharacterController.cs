using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    public abstract class BaseCharacterController : MonoBehaviour
    {
        [SerializeField] protected CharacterAttributes baseAttribute;
        protected abstract void Initialize();

        protected abstract void OnDie();

        protected abstract void OnHit();
    }
}
