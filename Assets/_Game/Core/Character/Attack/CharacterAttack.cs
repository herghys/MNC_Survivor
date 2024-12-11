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

        private void Awake()
        {
            DoOnAwake();
        }

        protected virtual void DoOnAwake()
        {
            CharacterAttributesController = GetComponent<CharacterAttributesController>();
        }

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
            BasicAttackSkill = skill;
            BasicAttackSpawnCount = spawnCount;
        }

        public virtual void SetupTarget(Transform target)
        {
            Target = target;
        }

        protected Vector3 GetRandomPositionAround(Transform characterTransform, float radius)
        {
            // Generate a random point in a 2D circle
            Vector2 randomPoint2D = UnityEngine.Random.insideUnitCircle * radius;

            // Convert it to a 3D position around the character
            Vector3 randomPosition = new Vector3(randomPoint2D.x, 0f, randomPoint2D.y) + characterTransform.position;

            return randomPosition;
        }

        protected abstract void BasicAttackTarget(BasicAttackSkill skill);

        public virtual IEnumerator IEBasicAttack(BasicAttackSkill skill)
        {
            while (!IsDead || !GameManager.Instance.IsPlayerDead)
            {
                if (skill.AttackTargetType == AttackTargetType.RandomPosition)
                {
                    yield return AttackRandomPosition(skill);
                }
                if (skill.AttackTargetType == AttackTargetType.OnSelf)
                {
                    yield return AttackFromSelf(skill);
                }
                if (skill.AttackTargetType == AttackTargetType.Target)
                {
                    yield return AttackToTarget(skill);
                }

                yield return new WaitForSeconds(skill.cooldown);
            }
        }
        protected abstract IEnumerator AttackRandomPosition(CharacterSkill skill);

        protected abstract IEnumerator AttackFromSelf(CharacterSkill skill);

        protected abstract VFXOwner GetVFXOwner();

        protected abstract IEnumerator AttackToTarget(CharacterSkill skill);

        protected abstract void AttackHomingTarget(CharacterSkill skill);

        protected abstract void AttackNonHomingTarget(CharacterSkill skill);
    }
}
