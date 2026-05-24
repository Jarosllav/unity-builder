using nobodyworks.builder.equipment;
using nobodyworks.builder.extensions;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace nobodyworks.builder.items
{
    [CreateAssetMenu(menuName = "Game/Definitions/Items/Item Definition", fileName = "Item_Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [SerializeField]
        private string _key = string.Empty;
        
        [SerializeField]
        private Sprite _icon;
        
        [SerializeField]
        private LocalizedString _name;
        
        [SerializeField]
        private LocalizedString _description;
        
        [SerializeField]
        private GameObject _prefab;
        
        [SerializeField]
        private ItemCategoryDefinition _categoryDefinition;
        
        [SerializeField]
        private ItemRarityDefinition _rarityDefinition;
        
        [SerializeField]
        private float _value = 0;
        
        [SerializeField]
        private bool _isStackable = true;
        
        [SerializeField]
        private bool _isEquippable = false;
        
        [SerializeField]
        private EquipmentSlotDefinition _slotDefinition;
        
        public string Key => _key;
        public Sprite Icon => _icon;
        public string Name => _name.GetText();
        public string Description => _description.GetText();
        public ItemCategoryDefinition CategoryDefinition => _categoryDefinition;
        public GameObject Prefab => _prefab;
        public ItemRarityDefinition RarityDefinition => _rarityDefinition;
        public float Value => _value;
        public bool IsStackable => _isStackable;
        public bool IsEquippable => _isEquippable;
        public EquipmentSlotDefinition SlotDefinition => _slotDefinition;

#if UNITY_EDITOR
        
        public LocalizedString Editor_LocalizedName => _name;
        
#endif
    }
}