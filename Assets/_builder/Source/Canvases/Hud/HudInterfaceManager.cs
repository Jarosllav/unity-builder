using System;
using TMPro;
using UnityEngine;

namespace nobodyworks.builder.interfaces
{
    public class HudInterfaceManager : InterfaceManager
    {
        [SerializeField]
        private QuickBarReferences _quickBarReferences;
        
        [SerializeField]
        private TMP_Text _dayTimeLabel;
        
        [SerializeField]
        private TMP_Text _dayPhaseLabel;
        
        protected override void OnInitialized()
        {
            _quickBarReferences.Setup(CharacterManager.InventoryController.Settings.MaxCapacity);
            
            CharacterManager.InventoryController.OnItemsChanged += () =>
            {
                _quickBarReferences.ResetAllSlots();

                for (int i = 0; i < CharacterManager.InventoryController.InventoryItems.Count; ++i)
                {
                    var invItem = CharacterManager.InventoryController.InventoryItems[i];

                    if (invItem == null)
                    {
                        continue;
                    }
                    
                    _quickBarReferences.SetSlot(i, invItem.Item.Definition);
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