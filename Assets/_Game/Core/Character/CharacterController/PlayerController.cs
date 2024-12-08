using HerghysStudio.Survivor.WorldGeneration;

using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    public class PlayerController : BaseCharacterController<PlayerMovement, PlayableCharacterData>
    {
        protected override void Initialize()
        {
            base.Initialize();
        }
        private void Start()
        {
            WorldGenerator.Instance.SetPlayer(this.transform); 
            CameraController.Instance.SetupPlayer(this.transform);
        }

        protected override void OnDie()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnHit()
        {
            throw new System.NotImplementedException();
        }
    }
}
