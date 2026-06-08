using UnityEngine;
using TriInspector;

namespace nobodyworks.builder.building
{
    [CreateAssetMenu(menuName = "Game/Databases/Building/Elements Database")]
    public class ElementsDatabase : ScriptableObject
    {
        [SerializeField]
        private ElementDefinition[] _definitions;
        
        [SerializeField]
        private ElementsCategoryDefinition[] _categoryDefinitions;
        
        public ElementDefinition[] Definitions => _definitions;
        public ElementsCategoryDefinition[] CategoryDefinitions => _categoryDefinitions;
        
#if UNITY_EDITOR

        [Button("Search")]
        private void Editor_GetAllDefinitions()
        {
            _definitions = utilities.Utils.GetAssets<ElementDefinition>();
            _categoryDefinitions = utilities.Utils.GetAssets<ElementsCategoryDefinition>();
            
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif
    }
}