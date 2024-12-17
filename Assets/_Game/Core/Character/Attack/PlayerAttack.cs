using System.Collections;

using HerghysStudio.Survivor.Spawner;
using HerghysStudio.Survivor.Utility.Coroutines;
using HerghysStudio.Survivor.VFX;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.Character
{
    public class PlayerAttack : CharacterAttack
    {
        [SerializeField] PlayerController controller;

        protected ObjectPool<AttackVFX> vfxPool;

        protected override void DoOnAwake()
        {
            controller = GetComponent<PlayerController>();
            base.DoOnAwake();
        }

        public override void Setup(BasicAttackSkill skill, int attackCount)
        {
            base.Setup(skill, attackCount);
            vfxPool = new(CreateVFX, GetVFX, ReleaseVFX, DestroyVFX);
        }

        protected override void OnGameStart()
        {
            BasicAttackTarget(BasicAttackSkill);
        }

        protected override void BasicAttackTarget(BasicAttackSkill skill)
        {
            IEBasicAttack(skill).Run();
        }

        private void LateUpdate()
        {
            IsDead = controller.IsDead;
        }

        protected override IEnumerator ProjectileToPosition(CharacterSkill skill, int count)
        {
            yield return null;
        }

        protected override IEnumerator ProjectileHoming(CharacterSkill skill, int count)
        {
            yield return null;
        }

        protected override IEnumerator SpawnAttackOnTarget(CharacterSkill skill, int count)
        {
            yield return null;
            for (int i = 0; i < count; i++)
            {
                if (IsDead || GameManager.Instance.IsPlayerDead)
                    break;

                //EnemySpawner.Instance.GetRa
                var attack = vfxPool.Get();

                attack.Setup(skill, skill.AttackVFXData, transform, GetVFXOwner(), CharacterAttributesController.DamageAttributes.Value);
                attack.SetupAsSpawned(false,false);
                attack.SetupTarget(transform, transform.position, transform, GetVFXOwner());
                attack.StartLogic();
            }
        }

        #region Pool
        protected override void DestroyVFX(AttackVFX vFX)
        {
            Destroy(vFX.gameObject);
        }

        protected override void ReleaseVFX(AttackVFX vFX)
        {
            vFX.gameObject.SetActive(false);
        }

        protected override void GetVFX(AttackVFX vFX)
        {
            vFX.SetupPool(vfxPool);
        }

        protected override AttackVFX CreateVFX()
        {
            var vfx = Instantiate(BasicAttackSkill.AttackVFXData.Prefab, GameManager.Instance.VFXHolder);
            vfx.gameObject.SetActive(false);
            vfx.SetupPool(vfxPool);
            return vfx;
        }
        #endregion

        #region Misc
        protected override VFXOwner GetVFXOwner()
        {
            return VFXOwner.Player;
        }

        protected override float GetDamage()
        {
            return CharacterAttributesController.DamageAttributes.Value;
        }

        protected override AttackVFX GetAttackVFX()
        {
            return vfxPool.Get();
        }
        #endregion
    }
}
