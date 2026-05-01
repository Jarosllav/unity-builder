using UnityEngine;

namespace nobodyworks
{
    public sealed class CharacterControllerDriver : MovementDriver
    {
        private readonly CharacterController _characterController;

        public CharacterControllerDriver(CharacterController characterController)
        {
            _characterController = characterController;
        }

        public override void ApplyMotion(Vector3 motion)
        {
            if (!_characterController.enabled)
            {
                return;
            }
            
            _characterController.Move(motion);
        }

        public override void DisableMotion()
        {
            _characterController.enabled = false;
        }

        public override void EnableMotion()
        {
            _characterController.enabled = true;
        }
    }
}
