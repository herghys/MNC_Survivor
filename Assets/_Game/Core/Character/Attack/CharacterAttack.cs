using System;
using System.Collections;

using HerghysStudio.Survivor.VFX;

using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    public abstract class CharacterAttack : MonoBehaviour
    {
        public BasicAttackSkill BasicAttackSkill { get; private set; }
        public int BasicAttackSpawnCount = 1;

        public CharacterAttributesController CharacterAttributesController { get; private set; }

        public Transform ProjectileSpawner;

        protected Transform Target;

        protected bool IsDead = false;
        protected bool GameEnded = false;
        private void Awake()
        {
            DoOnAwake();
        }

        protected virtual void OnEnable()
        {
            GameManager.Instance.OnGameStart += OnGameStart;
            GameManager.Instance.OnGameEnded += OnGameEnded;
        }



        protected virtual void OnDisable()
        {
            GameManager.Instance.OnGameStart -= OnGameStart;
            GameManager.Instance.OnGameEnded -= OnGameEnded;
        }


        protected virtual void DoOnAwake()
        {
            CharacterAttributesController = GetComponent<CharacterAttributesController>();
        }

        protected virtual void OnGameEnded(EndGameState arg0)
        {
            GameEnded = true;
            StopAllCoroutines();
        }

        protected abstract void OnGameStart();

        public void CharacterDied()
        {
            IsDead = true;
            StopAllCoroutines();
        }

        protected abstract AttackVFX CreateVFX();
        protected abstract void ReleaseVFX(AttackVFX vFX);
        protected abstract void GetVFX(AttackVFX vFX);
        protected abstract void DestroyVFX(AttackVFX vFX);


        public virtual void Setup(BasicAttackSkill skill, int spawnCount)
        {
            GameEnded = false;
            BasicAttackSkill = skill;
            BasicAttackSpawnCount = spawnCount;
        }

        public virtual void SetupTarget(Transform target)
        {
            Target = target;
        }

        protected abstract void BasicAttackTarget(BasicAttackSkill skill);

        public virtual IEnumerator IEBasicAttack(BasicAttackSkill skill)
        {
            while (!IsDead || !GameManager.Instance.IsPlayerDead)
            {
                if (skill.AttackTargetType == AttackTargetType.SpawnOnRandomPosition)
                {
                    yield return SpawnAttackOnRandomPosition(skill, BasicAttackSpawnCount);
                }
                if (skill.AttackTargetType == AttackTargetType.SpawnOnOpponentTarget)
                {
                    yield return SpawnAttackOnTarget(skill, BasicAttackSpawnCount);
                }
                if (skill.AttackTargetType == AttackTargetType.ProjectileToPosition)
                {
                    yield return ProjectileToPosition(skill, BasicAttackSpawnCount);
                }
                if (skill.AttackTargetType == AttackTargetType.HomingProjectile)
                {
                    yield return ProjectileHoming(skill, BasicAttackSpawnCount);
                }
                if (skill.AttackTargetType == AttackTargetType.SpawnOnSelf)
                {
                    yield return SpawnAttackOnSelf(skill, BasicAttackSpawnCount);
                }

                yield return new WaitForSeconds(skill.cooldown);
            }
        }

        protected abstract IEnumerator ProjectileToPosition(CharacterSkill skill, int count);

        protected abstract IEnumerator ProjectileHoming(CharacterSkill skill, int count);

        protected virtual IEnumerator SpawnAttackOnSelf(CharacterSkill skill, int count)
        {
            yield return null;
            for (int i = 0; i < count; i++)
            {
                if (IsDead || GameManager.Instance.IsPlayerDead || GameManager.Instance.IsGameEnded)
                    break;

                var attack = GetAttackVFX();

                attack.Setup(skill, skill.AttackVFXData, transform, GetVFXOwner(), CharacterAttributesController.DamageAttributes.Value);
                attack.SetupAsSpawned(true, false);
                attack.SetupTarget(transform, transform.position, transform, GetVFXOwner());
                attack.StartLogic();
            }
        }   

        protected virtual IEnumerator SpawnAttackOnRandomPosition(CharacterSkill skill, int count)
        {
            yield return null;
            for (int i = 0; i < count; i++)
            {
                if (IsDead || GameManager.Instance.IsPlayerDead || GameManager.Instance.IsGameEnded)
                    break;

                var randomPos = GetRandomPositionAround(transform, skill.maxRandomSpawnRange);
                var attack = GetAttackVFX();

                attack.Setup(skill, skill.AttackVFXData, transform, GetVFXOwner(), GetDamage());
                attack.SetupAsSpawned(false, true);
                attack.SetupTarget(transform, randomPos, transform, GetVFXOwner());
                attack.StartLogic();
            }
        }

        protected abstract IEnumerator SpawnAttackOnTarget(CharacterSkill skill, int count);


        #region Misc
        protected Vector3 GetRandomPositionAround(Transform characterTransform, float radius)
        {
            // Generate a random point in a 2D circle
            Vector2 randomPoint2D = UnityEngine.Random.insideUnitCircle * radius;

            // Convert it to a 3D position around the character
            Vector3 randomPosition = new Vector3(randomPoint2D.x, 0f, randomPoint2D.y) + characterTransform.position;

            return randomPosition;
        }

        protected abstract float GetDamage();

        protected abstract VFXOwner GetVFXOwner();

        protected abstract AttackVFX GetAttackVFX();
        #endregion
    }
}
