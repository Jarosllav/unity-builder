using UnityEngine;

namespace nobodyworks.builder.interfaces
{
    public class CharacterInterfaceManager : InterfaceManager
    {
        #region Inspector

        [SerializeField]
        private InventoryInterfaceManager _inventoryManager;

        #endregion

        protected override void OnInitialized()
        {
            _inventoryManager.Setup(CharacterManager.InventoryController);
            
            OnOpened += (_) => { _inventoryManager.Open(); };
            OnClosed += (_) => { _inventoryManager.Close(); };
        }
    }
}