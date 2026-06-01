using nobodyworks.builder.inventories;
using UnityEngine;

namespace nobodyworks.builder.interfaces
{
    public class CharacterInterfaceManager : InterfaceManager
    {
        #region Inspector

        [SerializeField]
        private InventoryInterfaceManager _inventoryManager;

        [SerializeField]
        private InventoryInterfaceManager _extInventoryManager;
        
        #endregion

        protected override void OnInitialized()
        {
            _inventoryManager.Setup(CharacterManager.InventoryController);
            
            OnOpened += (_) => { _inventoryManager.Open(); };
            OnClosed += (_) => { _inventoryManager.Close(); _extInventoryManager.Close(); };
            
            _extInventoryManager.Close();
        }

        public void Open(InventoryController extInventoryController)
        {
            _extInventoryManager.Setup(extInventoryController);
            
            Open();
            
            _extInventoryManager.Open();
        }
    }
}