using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace HerghysStudio.Survivor
{
    public class FaceCamera : MonoBehaviour
    {
        public Camera Camera;

        public bool FlipX;
        public bool FlipY;
        public bool FlipZ;

        private void Awake()
        {
            transform.localScale = new Vector3 (FlipX? -1 : 1, FlipY? -1 : 1, FlipZ? -1 : 1);
        }

        private void LateUpdate()
        {
            transform.LookAt(Camera.transform, Vector3.up);
        }
    }
}
