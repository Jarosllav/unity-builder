using System;
using UnityEngine;

namespace nobodyworks.builder.items
{
    [Serializable]
    public class ItemReference
    {
        [SerializeField]
        private ItemDefinition _definition;
        
        [SerializeField]
        private int _amount;
        
        public ItemDefinition Definition => _definition;
        public int Amount => _amount;
    }
}