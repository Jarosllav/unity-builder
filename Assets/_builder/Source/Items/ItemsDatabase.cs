using UnityEngine;

namespace nobodyworks.builder.items
{
    
    [CreateAssetMenu(menuName = "Game/Databases/Items database", fileName = "Items_Database")]
    public class ItemsDatabase : ScriptableObject
    {
        [SerializeField]
        private ItemDefinition[] _definitions;
        
        [SerializeField]
        private ItemRarityDefinition[] _rarityDefinitions;
        
        public ItemDefinition[] Definitions => _definitions;
        public ItemRarityDefinition[] RarityDefinitions => _rarityDefinitions;

        public ItemDefinition GetDefinition(string key)
        {
            foreach (var definition in _definitions)
            {
                if (definition.Key == key)
                {
                    return definition;
                }
            }
            
            return null;
        }

        public ItemRarityDefinition GetRarityDefinition(string key)
        {
            foreach (var rarityDefinition in _rarityDefinitions)
            {
                if (rarityDefinition.Key == key)
                {
                    return rarityDefinition;
                }
            }
            
            return null;
        }

#if UNITY_EDITOR

        [ContextMenu("Get all definitions")]
        private void Editor_GetAllDefinitions()
        {
            _definitions = utilities.Utils.GetAssets<ItemDefinition>();
            _rarityDefinitions = utilities.Utils.GetAssets<ItemRarityDefinition>();
            
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif
    }
}