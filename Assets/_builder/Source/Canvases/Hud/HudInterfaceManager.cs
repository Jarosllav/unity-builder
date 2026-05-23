using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace nobodyworks.builder.interfaces
{
    public class HudInterfaceManager : InterfaceManager
    {
        [SerializeField]
        private QuickBarWidget _quickBarWidget;
        
        [SerializeField]
        private KeyHintsWidget _keyHintsWidget;
        
        [SerializeField]
        private InteractableWidget _interactableWidget;
        
        [SerializeField]
        private TMP_Text _dayTimeLabel;
        
        [SerializeField]
        private TMP_Text _dayPhaseLabel;
        
        protected override void OnInitialized()
        {
            _quickBarWidget.Setup(CharacterManager.InventoryController.Settings.MaxCapacity);
            _keyHintsWidget.Setup(CharacterManager.InteractionController);
            _interactableWidget.Setup(CharacterManager.InteractionController, CharacterManager.CarrierController, 
                GameManager.PlayerInputProvider);
            
            CharacterManager.InventoryController.OnItemsChanged += () =>
            {
                _quickBarWidget.ResetAllSlots();

                for (int i = 0; i < CharacterManager.InventoryController.InventoryItems.Count; ++i)
                {
                    var invItem = CharacterManager.InventoryController.InventoryItems[i];

                    if (invItem == null)
                    {
                        continue;
                    }
                    
                    _quickBarWidget.SetSlot(i, invItem.Item.Definition);
                }
            };
        }

        public void Update()
        {
            _dayTimeLabel.text = $"{GameManager.ClockController.Hour:D2}:{GameManager.ClockController.Minute:D2}";
            _dayPhaseLabel.text = $"{GameManager.ClockController.Phase}";
        }
    }
}