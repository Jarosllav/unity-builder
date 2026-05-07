using System;
using nobodyworks.builder.equipment;
using nobodyworks.builder.movement;
using nobodyworks.builder.utilities;
using nobodyworks.builder.extensions;

namespace nobodyworks.builder.carrying
{
    public class CarrierController 
    {
        private readonly CarrierSettings _settings;
        private readonly EquipmentController _equipmentController;
        private readonly MovementController _movementController;

        private ICarryable _carrying;
        
        public Condition StartCondition { get; private set; } = new();
        public Condition EndCondition { get; private set; } = new();
        
        public ICarryable Carrying => _carrying;
        public bool IsCarrying => _carrying != null;
        
        public event Action OnCarryStarted;
        public event Action OnCarryEnded;
        
        public CarrierController(CarrierSettings settings, 
            EquipmentController equipmentController, MovementController movementController)
        {
            _settings = settings;
            _equipmentController = equipmentController;
            _movementController = movementController;
        }

        public void Tick(float deltaTime)
        {
            
        }
        
        public void Take(ICarryable carryable)
        {
            if (_carrying != null)
            {
                return;
            }
            
            var slotController = _equipmentController.GetSlotController(_settings.CarrySlotDefinition);

            if (slotController == null)
            {
                return;
            }

            if (!StartCondition.AllTrue())
            {
                return;
            }
            
            _carrying = carryable;
            _carrying.GameObject.transform.parent = slotController.BoneReference.Transform;
            _carrying.GameObject.transform.SetLocalOffset(_carrying.Offset);
            _carrying.CarryStarted();
            OnCarryStarted?.Invoke();
        }

        public void Drop()
        {
            if (_carrying == null)
            {
                return;
            }

            if (!EndCondition.AllTrue())
            {
                return;
            }
            
            _carrying.GameObject.transform.parent = null;
            _carrying.CarryEnded();
            OnCarryEnded?.Invoke();
            _carrying = null;
        }
    }
}