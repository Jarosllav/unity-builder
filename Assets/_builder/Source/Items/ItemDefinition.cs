using UnityEngine;

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
        private string _name = string.Empty; // TODO(PO): Implement localization package
        
        [SerializeField]
        private string _description = string.Empty; // TODO(PO): Implement localization package
        
        [SerializeField]
        private GameObject _prefab;
        
        [SerializeField]
        private ItemRarityDefinition _rarityDefinition;
        
        [SerializeField]
        private float _value = 0;
        
        public string Key => _key;
        public Sprite Icon => _icon;
        public string Name => _name;
        public string Description => _description;
        public GameObject Prefab => _prefab;
        public ItemRarityDefinition RarityDefinition => _rarityDefinition;
        public float Value => _value;
    }
}