using nobodyworks.builder.skeleton;
using UnityEngine;

namespace nobodyworks.builder.equipment
{
    [CreateAssetMenu(menuName = "Game/Definitions/Equipment/Equipment Slot Definition", fileName = "Slot_Definition")]
    public class EquipmentSlotDefinition : ScriptableObject
    {
        [SerializeField]
        private string _key;
        
        [SerializeField]
        private BoneDefinition _boneDefinition;
        
        public string Key => _key;
        public BoneDefinition BoneDefinition => _boneDefinition;
    }
}