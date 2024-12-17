using System.Collections;

using HerghysStudio.Survivor.Utility.Coroutines;
using HerghysStudio.Survivor.VFX;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.Character
{
    public class EnemyAttack : CharacterAttack
    {
        [SerializeField] EnemyController controller;

        protected ObjectPool<AttackVFX> vfxPool;

        protected override void DoOnAwake()
        {
            controller = GetComponent<EnemyController>();
            base.DoOnAwake();
            vfxPool = new(CreateVFX, GetVFX, ReleaseVFX, DestroyVFX);
        }

        protected override void BasicAttackTarget(BasicAttackSkill skill)
        {
            IEBasicAttack(skill).Run();
        }

        public void StartAttacking()
        {
            BasicAttackTarget(BasicAttackSkill);
        }

        #region Pool
        /// <summary>
        /// Destroy
        /// </summary>
        /// <param name="vFX"></param>
        protected override void DestroyVFX(AttackVFX vFX)
        {
            Destroy(vFX.gameObject);
        }

        /// <summary>
        /// Release
        /// </summary>
        /// <param name="vFX"></param>
        protected override void ReleaseVFX(AttackVFX vFX)
        {
            vFX.gameObject.SetActive(false);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="vFX"></param>
        protected override void GetVFX(AttackVFX vFX)
        {
            vFX.SetupPool(vfxPool);
            //vFX.Setup(vfx)
            //vFX.SetupTarget(Target, transform, VFXOwner.Enemy);
            vFX.gameObject.SetActive(true);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        protected override AttackVFX CreateVFX()
        {
            var vfx = Instantiate(BasicAttackSkill.AttackVFXData.Prefab, transform);
            vfx.SetupPool(vfxPool);
            return vfx;
        }
        #endregion

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

                var attack = vfxPool.Get();

                attack.Setup(skill, skill.AttackVFXData, transform, GetVFXOwner(), CharacterAttributesController.DamageAttributes.Value);
                attack.SetupAsSpawned(false, false);
            }
        }

        

        #region Misc
        protected override VFXOwner GetVFXOwner()
        {
            return VFXOwner.Enemy;
        }

        protected override float GetDamage()
        {
            return CharacterAttributesController.DamageAttributes.Value;
        }

        protected override AttackVFX GetAttackVFX()
        {
            return vfxPool.Get();
        }

        protected override void OnGameStart() { }
        #endregion
    }
}
