using nobodyworks.builder.items;
using TMPro;
using UnityEngine;

namespace nobodyworks.builder.interfaces
{
    public class ResourceSlotWidget : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private TMP_Text _label;

        #endregion

        public void Setup(ItemReference itemReference, int amount = 0)
        {
            _label.text = $"- {itemReference.Definition.Name} ({amount}/{itemReference.Amount})";
        }
    }
}