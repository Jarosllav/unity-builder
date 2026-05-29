using System;
using nobodyworks.builder.interaction;
using UnityEngine;

namespace nobodyworks.builder.items
{
    public class ItemInteractableManager : InteractableManager
    {
        #region Inspector

        [SerializeField]
        private ItemDefinition _itemDefinition;
        
        [SerializeField]
        private int _amount = 1;
        
        #endregion

        public int Amount => _amount;
        
        public Item GetItem()
        {
            if (Cookie is Item cachedItem)
            {
                return cachedItem;
            }
            
            return new Item(_itemDefinition);
        }
        
        protected override void OnUse(InteractionType interactionType)
        {
            GameObject.Destroy(gameObject);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if ((_name == null || _name.IsEmpty) && _itemDefinition != null)
            {
                _name = _itemDefinition.Editor_LocalizedName;
            }
        }

#endif
    }
}