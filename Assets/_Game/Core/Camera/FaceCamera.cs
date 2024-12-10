using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HerghysStudio.Survivor
{
    public class FaceCamera : MonoBehaviour
    {
        public Camera Camera;

        private void LateUpdate()
        {
            transform.LookAt(Camera.transform, Vector3.up);
        }
    }
}
