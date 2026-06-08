using nobodyworks.builder.items;
using UnityEngine;

namespace nobodyworks.builder.building
{
    [CreateAssetMenu(menuName = "Game/Definitions/Building/Element Definition", fileName = "Element_Definition")]
    public class ElementDefinition : ScriptableObject
    {
        #region Inspector

        [SerializeField]
        private GameObject _prefab;
        
        [SerializeField]
        private ItemReference[] _resources;
        
        [SerializeField]
        private ElementsCategoryDefinition _categoryDefinition;

        #endregion
        
        public GameObject Prefab => _prefab;
        public ItemReference[] Resources => _resources;
        public ElementsCategoryDefinition CategoryDefinition => _categoryDefinition;
    }
}