using nobodyworks.builder.extensions;
using nobodyworks.builder.items;
using UnityEngine;

namespace nobodyworks.builder.interfaces
{
    public class QuickBarWidget : MonoBehaviour
    {
        [SerializeField]
        private GameObject _slotPrefab;
        
        [SerializeField]
        private Transform _slotsTransform;

        internal void Setup(int maxSlots)
        {
            _slotsTransform.DestroyChildren();
            
            for (int i = 0; i < maxSlots; ++i)
            {
                var slotGameObject = GameObject.Instantiate(_slotPrefab, _slotsTransform);
                var slotWidget = slotGameObject.GetComponent<QuickSlotWidget>();
                
                slotWidget.Setup(i + 1);
            }
        }

        internal void SetSlot(int slotId, ItemDefinition itemDefinition)
        {
            var slotWidget = _slotsTransform.GetChild(slotId).gameObject.GetComponent<QuickSlotWidget>();
            slotWidget.Change(itemDefinition);
        }

        internal void ResetAllSlots()
        {
            for (int i = 0; i < _slotsTransform.childCount; ++i)
            {
                var slotWidget = _slotsTransform.GetChild(i).GetComponent<QuickSlotWidget>();
                slotWidget.Setup(i + 1);
            }
        }
    }
}