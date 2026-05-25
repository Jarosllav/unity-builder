using nobodyworks.builder.extensions;
using UnityEngine;
using UnityEngine.Localization;

namespace nobodyworks.builder.items
{
    [CreateAssetMenu(menuName = "Game/Definitions/Items/Item Rarity Definition", fileName = "Item_Rarity_Definition")]
    public class ItemRarityDefinition : ScriptableObject
    {
        [SerializeField]
        private string _key = string.Empty;
        
        [SerializeField]
        private LocalizedString _name;
        
        [SerializeField]
        private Color _color = Color.white;
        
        public string Key => _key;
        public string Name => _name.GetText();
        public Color Color => _color;
    }
}