using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HerghysStudio.Survivor
{
    [RequireComponent(typeof(LODGroup))]
    public class LODChecker : MonoBehaviour
    {
        [SerializeField] LODGroup lodGroup;
        [SerializeField] Rigidbody rb;

        private void Awake()
        {
            lodGroup ??= GetComponent<LODGroup>();
            rb ??=GetComponent<Rigidbody>();
        }

        private void LateUpdate()
        {
            if (lodGroup == null)
                return;

            if (rb == null)
                return;

            if (CheckIfCulled(lodGroup))
            {
                rb.isKinematic = true;
            }
            else
            {
                rb.isKinematic= false;
            }
        }


        /// <summary>
        /// Checks if the LODGroup is currently culled.
        /// </summary>
        /// <param name="lodGroup">The LODGroup to check.</param>
        /// <returns>True if the LODGroup is culled, otherwise False.</returns>
        private bool CheckIfCulled(LODGroup lodGroup)
        {
            // Get all LODs in the LODGroup
            LOD[] lods = lodGroup.GetLODs();

            foreach (LOD lod in lods)
            {
                foreach (Renderer renderer in lod.renderers)
                {
                    if (renderer != null && renderer.isVisible)
                    {
                        // If any renderer is visible, the LODGroup is not culled
                        return false;
                    }
                }
            }

            // If none of the renderers are visible, the LODGroup is culled
            return true;
        }

        private void Reset()
        {
            lodGroup ??= GetComponent<LODGroup>();
            rb ??= GetComponent<Rigidbody>();
        }
    }
}
