using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using nobodyworks.builder.inventories;

using Image = UnityEngine.UI.Image;

namespace nobodyworks.builder.interfaces
{
    public class InventorySlotWidget : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Inspector

        [SerializeField]
        private Image _icon;
        
        [SerializeField]
        private TMP_Text _amountLabel;
        
        #endregion
        
        private InventoryItem _inventoryItem;
        
        public void Setup(InventoryItem inventoryItem)
        {
            _inventoryItem = inventoryItem;

            if (_inventoryItem == null)
            {
                SetEmpty();
                return;
            }
            
            var itemDefinition = inventoryItem.Item.Definition;
            
            _icon.sprite = itemDefinition.Icon;
            _amountLabel.text = $"x{inventoryItem.Amount}";
        }

        private void SetEmpty()
        {
            _icon.sprite = null;
            _amountLabel.text = "";
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }
    }
}