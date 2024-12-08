using HerghysStudio.Survivor.Utility.Singletons;

using UnityEngine;

namespace HerghysStudio.Survivor
{
    public class CameraController : NonPersistentSingleton<CameraController>
    {
        public Cinemachine.CinemachineVirtualCamera VirtualCamera;

        public void SetupPlayer(Transform player)
        {
            VirtualCamera.LookAt = (player);
            VirtualCamera.Follow = (player);
        }
    }
}
