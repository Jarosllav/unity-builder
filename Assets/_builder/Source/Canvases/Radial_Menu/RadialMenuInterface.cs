using AillieoUtils.UI;
using nobodyworks.builder.building;
using nobodyworks.builder.character;
using nobodyworks.builder.extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace nobodyworks.builder.interfaces
{
    public class RadialMenuInterface : InterfaceManager
    {
        #region Inspector

        [SerializeField]
        private RadialLayoutGroup _slotsGroup;

        [SerializeField]
        private GameObject _slotPrefab;
        
        [SerializeField]
        private TMP_Text _nameLabel;
        
        [SerializeField]
        private Transform _resourcesSlotsGroup;
        
        [SerializeField]
        private GameObject _resourcesSlotPrefab;
        
        #endregion
        
        private ElementsDatabase _elementsDatabase;
        private RadialSlotWidget _selectedSlot;

        protected override void OnInitialized()
        {
            _elementsDatabase = Databases.Elements;
            CreateSlots();
            Close();
            
            OnClosed += _ =>
            {
                CharacterManager.BuilderController.SetElement(_selectedSlot.Definition);
                _nameLabel.text = string.Empty;
                CreateResourcesSlots(null);
            };
        }

        private void CreateSlots()
        {
            _slotsGroup.transform.DestroyChildren();
            
            var elementsCount = _elementsDatabase.Definitions.Length;
            var elementGap = 360f / elementsCount / 2f;

            for (int i = 0; i < elementsCount; i++)
            {
                var definition = _elementsDatabase.Definitions[i];
                
                var slotGameObject = Instantiate(_slotPrefab, _slotsGroup.transform);
                var slotWidget = slotGameObject.GetComponent<RadialSlotWidget>();
                
                slotWidget.Segment.SetAngles(-elementGap, elementGap);
                slotWidget.Definition = definition;
                
                slotWidget.OnSelected += SlotWidgetSelectedHandler;
            }
            
            _slotsGroup.AngleDelta = 360f / elementsCount;
        }

        private void CreateResourcesSlots(ElementDefinition definition)
        {
            _resourcesSlotsGroup.DestroyChildren();

            if (definition == null)
            {
                return;
            }

            var inventoryController = CharacterManager.InventoryController;
            
            foreach (var itemReference in definition.Resources)
            {
                var slotGameObject = Instantiate(_resourcesSlotPrefab, _resourcesSlotsGroup);
                var slotWidget = slotGameObject.GetComponent<ResourceSlotWidget>();
                slotWidget.Setup(itemReference, inventoryController.Amount(itemReference.Definition));
            }
        }

        private void SlotWidgetSelectedHandler(RadialSlotWidget widget)
        {
            _selectedSlot = widget;
            _nameLabel.text = widget.Definition.Name;
            CreateResourcesSlots(widget.Definition);
        }
    }
}