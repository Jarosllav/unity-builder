using UnityEngine;

namespace nobodyworks.builder.items
{
    [CreateAssetMenu(menuName = "Game/Definitions/Items/Item Rarity Definition", fileName = "Item_Rarity_Definition")]
    public class ItemRarityDefinition : ScriptableObject
    {
        [SerializeField]
        private string _key = string.Empty;
        
        [SerializeField]
        private string _name = string.Empty; // TODO: Implement localization package
        
        [SerializeField]
        private Color _color = Color.white;
        
        public string Key => _key;
        public string Name => _name;
        public Color Color => _color;
    }
}