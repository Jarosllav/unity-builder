using System;
using nobodyworks.builder.items;

namespace nobodyworks.builder.quickbar
{
    public class QuickBarController
    {
        private readonly QuickBarSettings _settings;
        private readonly ItemDefinition[] _slots;

        public int Capacity => _settings.Capacity;

        public event Action<int, ItemDefinition> OnSlotChanged;

        #region Initialization

        public QuickBarController(QuickBarSettings settings)
        {
            _settings = settings;
            _slots = new ItemDefinition[_settings.Capacity];
        }

        public void Dispose()
        {
            OnSlotChanged = null;
        }

        #endregion

        public void Assign(int slotIndex, ItemDefinition itemDefinition)
        {
            if (slotIndex < 0 || slotIndex >= _slots.Length)
            {
                return;
            }

            _slots[slotIndex] = itemDefinition;
            OnSlotChanged?.Invoke(slotIndex, itemDefinition);
        }

        public void Clear(int slotIndex)
        {
            Assign(slotIndex, null);
        }

        public ItemDefinition GetSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _slots.Length)
            {
                return null;
            }

            return _slots[slotIndex];
        }
    }
}
