using nobodyworks.builder.items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nobodyworks.builder.interfaces
{
    public class QuickSlotReferences : MonoBehaviour
    {
        [SerializeField]
        private Image _iconImage;
        
        [SerializeField]
        private TMP_Text _numLabel;

        internal void Setup(int id)
        {
            _iconImage.sprite = null;
            _iconImage.enabled = false;
            _numLabel.text = id.ToString();
        }

        internal void Change(ItemDefinition itemDefinition)
        {
            _iconImage.sprite = itemDefinition.Icon;
            _iconImage.enabled = true;
        }
    }
}