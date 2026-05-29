using nobodyworks.builder.items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nobodyworks.builder.interfaces
{
    public class QuickSlotWidget : MonoBehaviour
    {
        [SerializeField]
        private Image _iconImage;

        [SerializeField]
        private TMP_Text _numLabel;

        public void Setup(int id, ItemDefinition itemDefinition = null)
        {
            _numLabel.text = id.ToString();
            Change(itemDefinition);
        }

        public void Change(ItemDefinition itemDefinition)
        {
            if (itemDefinition == null)
            {
                _iconImage.sprite = null;
                _iconImage.enabled = false;
                return;
            }

            _iconImage.sprite = itemDefinition.Icon;
            _iconImage.enabled = true;
        }
    }
}
