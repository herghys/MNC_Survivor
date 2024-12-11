using System.Collections;

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

        private void OnEnable()
        {
            GameManager.Instance.OnGameStart += OnGameStart;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnGameStart -= OnGameStart;

        }

        public override void Setup(BasicAttackSkill skill, int attackCount)
        {
            base.Setup(skill, attackCount);
            vfxPool = new(CreateVFX, GetVFX, ReleaseVFX, DestroyVFX);
        }

        private void OnGameStart()
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

        protected override IEnumerator AttackToTarget(CharacterSkill skill)
        {
            yield return null;
        }

        protected override void AttackHomingTarget(CharacterSkill skill)
        {
            for (int i = 0; i < BasicAttackSpawnCount; i++)
            {
                if (IsDead || !GameManager.Instance.IsPlayerDead)
                    break;
                AttackVFX vfx = vfxPool.Get();

                vfx.transform.position = transform.position;
                //vfx.Setup(skill.AttackVFXData, EnemySpawner.Instance., VFXOwner.Player, CharacterAttributesController.DamageAttributes.Value);
            }
        }

        protected override void AttackNonHomingTarget(CharacterSkill skill)
        {
            for (int i = 0; i < BasicAttackSpawnCount; i++)
            {
                if (IsDead || !GameManager.Instance.IsPlayerDead)
                    break;
                // Retrieve a projectile from the pool
                AttackVFX vfx = vfxPool.Get();

                // Set the starting position to the character's current position
                vfx.transform.position = transform.position;

                // Determine the forward direction
                Vector3 forwardDirection = transform.forward;

                // Setup the VFX with direction or target
                vfx.Setup(skill.AttackVFXData, forwardDirection, transform, VFXOwner.Player, CharacterAttributesController.DamageAttributes.Value);
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
            vFX.Pool = vfxPool;
            vFX.transform.parent = GameManager.Instance.VFXHolder;
            vFX.Target = Target;
            vFX.gameObject.SetActive(true);
        }

        protected override AttackVFX CreateVFX()
        {
            var vfx = Instantiate(BasicAttackSkill.AttackVFXData.Prefab, GameManager.Instance.VFXHolder);
            vfx.Pool = vfxPool;
            return vfx;
        }
        #endregion

        protected override VFXOwner GetVFXOwner()
        {
            return VFXOwner.Player;
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

                if (vfx == null)
                    yield break;

                vfx.transform.position = transform.position;
                vfx.Setup(skill.AttackVFXData, null, transform, GetVFXOwner(), CharacterAttributesController.DamageAttributes.Value, true);

                yield return null;
            }
        }
    }
}
