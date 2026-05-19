using System;
using TMPro;
using UnityEngine;

namespace nobodyworks.builder.interfaces
{
    public class KeyHintReferences : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private TMP_Text _keyLabel;
        
        [SerializeField]
        private TMP_Text _infoLabel;

        #endregion

        public void Awake()
        {
            _keyLabel.text = string.Empty;
            _infoLabel.text = string.Empty;
        }

        public void Setup(string keyLabel, string infoLabel)
        {
            _keyLabel.text = keyLabel;
            _infoLabel.text = infoLabel;
        }
    }
}