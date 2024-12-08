using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    public class EnemyController : BaseCharacterController<EnemyMovement, EnemyCharacterData>
    {
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void OnDie()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnHit()
        {
            throw new System.NotImplementedException();
        }
    }
}
