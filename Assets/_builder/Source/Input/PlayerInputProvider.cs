using System;
using nobodyworks.builder.character;
using nobodyworks.builder.interaction;
using nobodyworks.builder.interfaces;
using nobodyworks.builder.inventories;
using nobodyworks.builder.items;
using nobodyworks.builder.movement;
using nobodyworks.builder.placement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace nobodyworks.builder.input
{
    public enum InputMode
    {
        Gameplay,
        UI
    }
    
    public class PlayerInputProvider : MonoBehaviour, IInputProvider
    {
        #region Inspector

        [SerializeField]
        private CharacterController _characterController;

        #endregion
        
        private InputSystem_Actions _actionAsset;
        private InputMode _inputMode = InputMode.Gameplay;
        
        private CanvasManager _canvasManager;
        private CharacterManager _characterManager;
        private MovementController _movementController;
        private PlacementController _placementController;
        
        public event Action<InteractionType, float> OnInteractionStarted;
        public event Action<InteractionType> OnInteractionCanceled;
        
        public void Awake()
        {
            _actionAsset = new();
            _characterManager = GetComponent<CharacterManager>();
            
            Assert.IsNotNull(_characterManager);

            if (_characterManager.IsInstalled)
            {
                CharacterInstalledHandler();
            }
            else
            {
                _characterManager.OnInstalled += CharacterInstalledHandler;
            }
        }

        public void Start()
        {
            _canvasManager = CanvasManager.Instance;
            
            _canvasManager.OnCursorChanged += () =>
            {
                if (Cursor.lockState == CursorLockMode.None)
                {
                    SetMode(InputMode.UI);
                }
                else
                {
                    SetMode(InputMode.Gameplay);
                }
            };
        }

        public void OnDestroy()
        {
            OnInteractionStarted = null;
            OnInteractionCanceled = null;
        }

        public void SetMode(InputMode inputMode)
        {
            _inputMode = inputMode;

            if (_inputMode == InputMode.Gameplay)
            {
                _actionAsset.UI.Disable();
                _actionAsset.Player.Enable();
                
                _movementController.Constraints = MovementConstraint.None;
            }
            else if (_inputMode == InputMode.UI)
            {
                _actionAsset.Player.Disable();
                _actionAsset.UI.Enable();
                
                _movementController.Constraints = MovementConstraint.FullMotion;
            }
        }
        
        private void CharacterInstalledHandler()
        {
            var movementDriver = new CharacterControllerDriver(_characterController);

            _movementController = _characterManager.MovementController;
            _movementController.SetDriver(movementDriver);
            
            _placementController = _characterManager.PlacementController;
            
            _actionAsset.Enable();
            
            Cursor.lockState = CursorLockMode.Locked;
            
            CreateEventHandlers();
            CreateGlobalEventHandlers();
            
            _characterManager.InteractionController.Register<CrateInteractableManager>((crateManager, interactionType) =>
            {
                if (interactionType == InteractionType.Primary)
                {
                    _canvasManager.GetInterface<CharacterInterfaceManager>().Open(crateManager.InventoryController);
                }
            });
        }

        private void CreateEventHandlers()
        {
            _actionAsset.Player.Interact_Primary.started += (ctx) => InteractionStart(InteractionType.Primary, (ctx.interaction as HoldInteraction).duration);
            _actionAsset.Player.Interact_Primary.canceled += (ctx) => InteractionCancel(InteractionType.Primary);
            _actionAsset.Player.Interact_Primary.performed += (ctx) => Interact(InteractionType.Primary);
            
            _actionAsset.Player.Interact_Secondary.started += (ctx) => InteractionStart(InteractionType.Secondary, (ctx.interaction as HoldInteraction).duration);
            _actionAsset.Player.Interact_Secondary.canceled += (ctx) => InteractionCancel(InteractionType.Secondary);
            _actionAsset.Player.Interact_Secondary.performed += (ctx) => Interact(InteractionType.Secondary);
            
            _actionAsset.Player.Action_Primary.performed += (ctx) => Interact(InteractionType.PrimaryAction);
            _actionAsset.Player.Action_Secondary.performed += (ctx) => Interact(InteractionType.SecondaryAction);
            
            _actionAsset.Player.Jump.performed += (ctx) => _characterManager.MovementController.Jump();
        }

        private void CreateGlobalEventHandlers()
        {
            _actionAsset.Global.Tab.performed += (ctx) => _canvasManager.GetInterface<CharacterInterfaceManager>().Toggle();

            _actionAsset.Global.Quick_1.performed += (ctx) => Quick(0);
            _actionAsset.Global.Quick_2.performed += (ctx) => Quick(1);
            _actionAsset.Global.Quick_3.performed += (ctx) => Quick(2);
            _actionAsset.Global.Quick_4.performed += (ctx) => Quick(3);
            _actionAsset.Global.Quick_5.performed += (ctx) => Quick(4);
            _actionAsset.Global.Quick_6.performed += (ctx) => Quick(5);
            _actionAsset.Global.Quick_7.performed += (ctx) => Quick(6);
            _actionAsset.Global.Quick_8.performed += (ctx) => Quick(7);
            _actionAsset.Global.Quick_9.performed += (ctx) => Quick(8);
            _actionAsset.Global.Quick_0.performed += (ctx) => Quick(9);
            
            
            _actionAsset.Global.Radial.performed += (ctx) =>
            {
                if (_characterManager.BuilderController.IsEnabled)
                {
                    _canvasManager.GetInterface<RadialMenuInterface>().Open();
                }
            };
            _actionAsset.Global.Radial.canceled += (ctx) =>
            {
                if (_characterManager.BuilderController.IsEnabled)
                {
                    _canvasManager.GetInterface<RadialMenuInterface>().Close();
                }
            };
        }

        public void Update()
        {
            if (_movementController == null)
            {
                return;
            }
            
            _movementController.Move(GetMove());
            _movementController.RotateDelta(GetLook());

            if (_actionAsset.Player.Rotate_Left.inProgress)
            {
                _placementController.Rotate(false);
            }
            else if (_actionAsset.Player.Rotate_Right.inProgress)
            {
                _placementController.Rotate(true);
            }
        }

        public Vector2 GetMove()
        {
            return _actionAsset.Player.Move.ReadValue<Vector2>();
        }
        
        public Vector2 GetLook()
        {
            return _actionAsset.Player.Look.ReadValue<Vector2>();
        }

        private void Interact(InteractionType interactionType)
        {
            if (interactionType == InteractionType.Secondary && _characterManager.CarrierController.IsCarrying)
            {
                _characterManager.CarrierController.Drop();
                return;
            }

            if (interactionType == InteractionType.PrimaryAction && _characterManager.BuilderController.IsEnabled)
            {
                _characterManager.BuilderController.TryPlace();
                return;
            }
            
            _characterManager.InteractionController.Use(interactionType);
        }

        private void InteractionStart(InteractionType interactionType, float duration)
        {
            OnInteractionStarted?.Invoke(interactionType, duration > 0f ? duration : InputSystem.settings.defaultHoldTime);
        }
        
        private void InteractionCancel(InteractionType interactionType)
        {
            OnInteractionCanceled?.Invoke(interactionType);
        }
        
        private void Quick(int id)
        {
            if (_inputMode == InputMode.UI)
            {
                AssignQuickSlot(id);
            }
            else
            {
                EquipFromQuickSlot(id);
            }
        }

        private void AssignQuickSlot(int id)
        {
            var inventoryInterface = _canvasManager.GetInterface<CharacterInterfaceManager>().InventoryManager;
            
            if (inventoryInterface == null || !inventoryInterface.IsHoveringSlot)
            {
                return;
            }

            var hoveredItem = inventoryInterface.HoveredItem;

            if (hoveredItem == null)
            {
                _characterManager.QuickBarController.Clear(id);
                return;
            }

            if (!hoveredItem.Item.Definition.IsEquippable)
            {
                return;
            }

            if (_characterManager.QuickBarController.GetSlot(id) == hoveredItem.Item.Definition)
            {
                _characterManager.QuickBarController.Clear(id);
                return;
            }

            _characterManager.QuickBarController.Assign(id, hoveredItem.Item.Definition);
        }

        private void EquipFromQuickSlot(int id)
        {
            var itemDefinition = _characterManager.QuickBarController.GetSlot(id);
            if (itemDefinition == null)
            {
                return;
            }

            var inventoryItem = FindInventoryItem(itemDefinition);
            if (inventoryItem == null)
            {
                return;
            }

            if (_characterManager.EquipmentController.IsEquipped(inventoryItem.Item))
            {
                _characterManager.EquipmentController.Unequip(inventoryItem.Item);
            }
            else
            {
                _characterManager.EquipmentController.Equip(inventoryItem.Item);
            }
        }

        private InventoryItem FindInventoryItem(ItemDefinition itemDefinition)
        {
            foreach (var invItem in _characterManager.InventoryController.InventoryItems)
            {
                if (invItem != null && invItem.Item.Definition == itemDefinition)
                {
                    return invItem;
                }
            }

            return null;
        }
    }
}