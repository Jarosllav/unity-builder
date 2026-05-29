using System;
using nobodyworks.builder.inventories;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using Image = UnityEngine.UI.Image;

namespace nobodyworks.builder.interfaces
{
    public class InventorySlotWidget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Inspector

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TMP_Text _amountLabel;
        
        [SerializeField]
        private GameObject _amountBackgroundGameObject;

        #endregion

        private InventoryItem _inventoryItem;

        public InventoryItem InventoryItem => _inventoryItem;
        public Action<InventorySlotWidget> OnHoverEnter;
        public Action<InventorySlotWidget> OnHoverExit;

        public void Setup(InventoryItem inventoryItem)
        {
            _inventoryItem = inventoryItem;

            if (_inventoryItem == null)
            {
                SetEmpty();
                return;
            }

            _icon.sprite = inventoryItem.Item.Definition.Icon;
            _icon.enabled = true;

            _amountLabel.gameObject.SetActive(inventoryItem.Amount > 1);
            _amountBackgroundGameObject.SetActive(inventoryItem.Amount > 1);
            
            _amountLabel.text = $"x{inventoryItem.Amount}";
        }

        private void SetEmpty()
        {
            _icon.sprite = null;
            _icon.enabled = false;
            _amountLabel.text = "";
            
            _amountLabel.gameObject.SetActive(false);
            _amountBackgroundGameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHoverEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnHoverExit?.Invoke(this);
        }
    }
}
