using System.Collections;
using System.Collections.Generic;

using HerghysStudio.Survivor.VFX;

using UnityEngine;

namespace HerghysStudio.Survivor.VFX
{
    [CreateAssetMenu(fileName = "Attack VFX Data", menuName = "VFX/Data")]
    public class AttackVFXData : ScriptableObject
    {
        public AttackVFX Prefab;
        public LayerMask LayerMask;
        public List<AttackVFXBehaviour> Behaviours = new List<AttackVFXBehaviour>();
    }
}
