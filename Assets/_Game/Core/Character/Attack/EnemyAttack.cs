using System.Collections;
using System.Collections.Generic;

using HerghysStudio.Survivor.Utility.Coroutines;
using HerghysStudio.Survivor.VFX;

using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    public class EnemyAttack : CharacterAttack
    {
        [SerializeField] EnemyController controller;

        protected override void DoOnAwake()
        {
            controller = GetComponent<EnemyController>();
            base.DoOnAwake();
            vfxPool = new(CreateVFX, GetVFX, ReleaseVFX, DestroVFX);
        }

        public override void BasicAttackTarget(BasicAttackSkill skill)
        {
            IEBasicAttack(skill).Run();
        }

        public override IEnumerator IEBasicAttack(BasicAttackSkill skill)
        {
            while (!controller.IsDead)
            {
                yield return new WaitForSeconds(skill.cooldown);
            }
        }

        protected override void DestroVFX(AttackVFX vFX)
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
            vFX.transform.parent = vfxHolder;
            vFX.Target = Target;
            vFX.gameObject.SetActive(true);
        }

        protected override AttackVFX CreateVFX()
        {
            var vfx = BasicAttackSkill.AttackVFXData.Prefab;
            vfx.Pool = vfxPool;
            return vfx;
        }
    }
}
