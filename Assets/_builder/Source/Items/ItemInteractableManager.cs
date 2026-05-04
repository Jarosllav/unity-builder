using nobodyworks.builder.interaction;
using UnityEngine;

namespace nobodyworks.builder.items
{
    public class ItemInteractableManager : InteractableManager
    {
        #region Inspector

        [SerializeField]
        private ItemDefinition _itemDefinition;
        
        #endregion

        public Item GetItem()
        {
            if (Cookie is Item cachedItem)
            {
                return cachedItem;
            }
            
            return new Item(_itemDefinition);
        }
        
        protected override void OnUse()
        {
            GameObject.Destroy(gameObject);
        }
    }
}