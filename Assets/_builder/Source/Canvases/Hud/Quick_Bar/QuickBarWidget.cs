using nobodyworks.builder.extensions;
using nobodyworks.builder.items;
using nobodyworks.builder.quickbar;
using UnityEngine;

namespace nobodyworks.builder.interfaces
{
    public class QuickBarWidget : MonoBehaviour
    {
        [SerializeField]
        private GameObject _slotPrefab;

        [SerializeField]
        private Transform _slotsTransform;

        public void Setup(QuickBarController controller)
        {
            _slotsTransform.DestroyChildren();

            for (int i = 0; i < controller.Capacity; ++i)
            {
                var slotGameObject = GameObject.Instantiate(_slotPrefab, _slotsTransform);
                var slotWidget = slotGameObject.GetComponent<QuickSlotWidget>();
                slotWidget.Setup(i + 1, controller.GetSlot(i));
            }

            controller.OnSlotChanged += SlotChangedHandler;
        }

        private void SlotChangedHandler(int slotIndex, ItemDefinition itemDefinition)
        {
            var slotWidget = _slotsTransform.GetChild(slotIndex).GetComponent<QuickSlotWidget>();
            slotWidget.Change(itemDefinition);
        }
    }
}
