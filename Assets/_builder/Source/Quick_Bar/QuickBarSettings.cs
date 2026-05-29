using System;
using UnityEngine;

namespace nobodyworks.builder.quickbar
{
    [Serializable]
    public class QuickBarSettings
    {
        [SerializeField, Min(1)]
        private int _capacity = 4;

        public int Capacity => _capacity;
    }
}
