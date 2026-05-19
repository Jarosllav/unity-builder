using nobodyworks.builder.extensions;
using nobodyworks.builder.items;
using UnityEngine;

namespace nobodyworks.builder.interfaces
{
    public class QuickBarReferences : MonoBehaviour
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
                var slotReferences = slotGameObject.GetComponent<QuickSlotReferences>();
                
                slotReferences.Setup(i + 1);
            }
        }

        internal void SetSlot(int slotId, ItemDefinition itemDefinition)
        {
            var slotReferences = _slotsTransform.GetChild(slotId).gameObject.GetComponent<QuickSlotReferences>();
            slotReferences.Change(itemDefinition);
        }

        internal void ResetAllSlots()
        {
            for (int i = 0; i < _slotsTransform.childCount; ++i)
            {
                var slotReference = _slotsTransform.GetChild(i).GetComponent<QuickSlotReferences>();
                slotReference.Setup(i + 1);
            }
        }
    }
}