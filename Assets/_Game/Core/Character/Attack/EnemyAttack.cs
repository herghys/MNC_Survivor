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


        private void OnEnable()
        {
            GameManager.Instance.OnGameEnded += OnGameEnded;
        }



        private void OnDisable()
        {
            GameManager.Instance.OnGameEnded -= OnGameEnded;

        }
        private void OnGameEnded(bool arg0)
        {
            StopAllCoroutines();
        }

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
            vFX.Pool = vfxPool;
            vFX.transform.parent = transform;
            vFX.transform.position = transform.position;
            vFX.Target = Target;
            vFX.gameObject.SetActive(true);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        protected override AttackVFX CreateVFX()
        {
            var vfx = Instantiate(BasicAttackSkill.AttackVFXData.Prefab, transform);
            vfx.Pool = vfxPool;
            return vfx;
        }
        #endregion

        protected override IEnumerator AttackToTarget(CharacterSkill skill)
        {
            yield return null;
        }

        protected override void AttackHomingTarget(CharacterSkill skill)
        {
            throw new System.NotImplementedException();
        }

        protected override void AttackNonHomingTarget(CharacterSkill skill)
        {
            throw new System.NotImplementedException();
        }

        protected override VFXOwner GetVFXOwner()
        {
            return VFXOwner.Enemy;
        }

        protected override IEnumerator AttackRandomPosition(CharacterSkill skill)
        {
            for (int i = 0; i < BasicAttackSpawnCount; i++)
            {
                if (IsDead || GameManager.Instance.IsPlayerDead)
                    break;

                var randomPos = GetRandomPositionAround(transform, skill.maxRandomSpawnRange);
                var vfx = vfxPool.Get();
                if (vfx == null)
                    yield break;

                vfx.transform.position = new Vector3(randomPos.x, vfx.transform.position.y, randomPos.z);
                vfx.Setup(skill.AttackVFXData, null, transform, GetVFXOwner(), CharacterAttributesController.DamageAttributes.Value);

                vfx.transform.parent = GameManager.Instance.VFXHolder;

                yield return null;
            }
        }

        protected override IEnumerator AttackFromSelf(CharacterSkill skill)
        {
            for (int i = 0; i < BasicAttackSpawnCount; i++)
            {
                if (IsDead || GameManager.Instance.IsPlayerDead)
                    break;

                var vfx = vfxPool.Get();
                vfx.transform.position = transform.position;

                if (vfx == null)
                    yield break;

                vfx.transform.position = transform.position;
                vfx.Setup(skill.AttackVFXData, null, transform, GetVFXOwner(), CharacterAttributesController.DamageAttributes.Value, true);

                vfx.transform.parent = GameManager.Instance.VFXHolder;
                yield return null;
            }
        }
    }
}
