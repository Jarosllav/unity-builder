using System;
using nobodyworks.builder.character;
using nobodyworks.builder.interaction;
using nobodyworks.builder.movement;
using UnityEngine;

namespace nobodyworks.builder.cutscene
{
    public class CarriageCutsceneManager : CutsceneManager
    {
        #region Inspector
        
        [SerializeField]
        private CharacterManager _characterManager;

        [SerializeField]
        private Transform _passengerSeat;

        #endregion

        private readonly Func<InteractableManager, bool> _interactionLock = (_) => false;

        protected override void OnSetup()
        {
            var movementController = _characterManager.MovementController;
            var interactionController = _characterManager.InteractionController;

            _characterManager.transform.SetParent(_passengerSeat);
            _characterManager.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            movementController.Constraints = MovementConstraint.Motion | MovementConstraint.Gravity;
            interactionController.UseCondition.Subscribe(_interactionLock);
        }

        protected override void OnTeardown()
        {
            var movementController = _characterManager.MovementController;
            var interactionController = _characterManager.InteractionController;

            _characterManager.transform.SetParent(null, worldPositionStays: true);

            movementController.Constraints = MovementConstraint.None;
            interactionController.UseCondition.Unsubscribe(_interactionLock);
        }
    }
}
