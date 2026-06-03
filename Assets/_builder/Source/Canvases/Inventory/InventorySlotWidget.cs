using System;
using nobodyworks.builder.inventories;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using Image = UnityEngine.UI.Image;

namespace nobodyworks.builder.interfaces
{
    public class InventorySlotWidget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
        IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        #region Inspector

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TMP_Text _amountLabel;
        
        [SerializeField]
        private GameObject _amountBackgroundGameObject;

        #endregion

        private int _index;
        private InventoryInterfaceManager _owner;
        private InventoryItem _inventoryItem;

        public int Index => _index;
        public InventoryInterfaceManager Owner => _owner;
        public InventoryItem InventoryItem => _inventoryItem;
        
        public Action<InventorySlotWidget> OnHoverEnter;
        public Action<InventorySlotWidget> OnHoverExit;
        public Action<InventorySlotWidget> OnDropped;

        public void Setup(int index, InventoryInterfaceManager owner, InventoryItem inventoryItem)
        {
            _index = index;
            _owner = owner;
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

        public void OnDestroy()
        {
            OnHoverEnter = null;
            OnHoverExit = null;
            OnDropped = null;
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
            if (_inventoryItem != null)
            {
                TooltipManager.Instance.Show(_inventoryItem.Item.Definition.Name);
            }
            
            OnHoverEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.Hide();
            
            OnHoverExit?.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DraggableManager.Instance.Show(_icon.gameObject, this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DraggableManager.Instance.Hide();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (DraggableManager.Instance.Cookie is not InventorySlotWidget sourceSlotWidget)
            {
                return;
            }
            
            /*var invItem = sourceSlotWidget.InventoryItem;
            sourceSlotWidget.Setup(null);
            Setup(invItem);*/
            
            OnDropped?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }
    }
}
