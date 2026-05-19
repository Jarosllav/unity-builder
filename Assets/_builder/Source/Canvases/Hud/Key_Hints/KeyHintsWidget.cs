using System;
using System.Collections.Generic;
using nobodyworks.builder.extensions;
using nobodyworks.builder.interaction;
using nobodyworks.builder.utilities;
using UnityEngine;

namespace nobodyworks.builder.interfaces
{
    public class KeyHintsWidget : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private GameObject _keyHintPrefab;

        [SerializeField]
        private Transform _keyHintTransform;

        [SerializeField]
        private string _primaryKeyLabel = "E";

        [SerializeField]
        private string _secondaryKeyLabel = "F";

        [SerializeField]
        private string _forceKeyLabel = "G";

        #endregion

        private Pool<KeyHintWidget> _hintsPool;
        private readonly List<KeyHintWidget> _activeHints = new();

        public void Awake()
        {
            _keyHintTransform.DestroyChildren();
            _hintsPool = new(CreateKeyHint, 6);
        }

        public void Setup(InteractionController interactionController)
        {
            interactionController.OnSelected += InteractionSelectedHandler;
            interactionController.OnDeselected += InteractionDeselectedHandler;
        }

        private KeyHintWidget CreateKeyHint()
        {
            var hintGameObject = GameObject.Instantiate(_keyHintPrefab, _keyHintTransform);
            var hintWidget = hintGameObject.GetComponent<KeyHintWidget>();
            
            return hintWidget;
        }

        private void InteractionSelectedHandler(InteractionController _, InteractableManager interactableManager)
        {
            var types = interactableManager.InteractionTypes;
            
            foreach (InteractionType type in Enum.GetValues(typeof(InteractionType)))
            {
                if (type == InteractionType.None)
                {
                    continue;
                }
                
                if ((types & type) == 0)
                {
                    continue;
                }

                var hint = _hintsPool.Get();
                _activeHints.Add(hint);
                hint.Setup(GetKeyLabel(type), interactableManager.GetInteractionLabel(type));
            }
        }

        private void InteractionDeselectedHandler(InteractionController _, InteractableManager interactableManager)
        {
            foreach (var hint in _activeHints)
            {
                _hintsPool.Return(hint);
            }
            
            _activeHints.Clear();
        }

        private string GetKeyLabel(InteractionType type) => type switch
        {
            InteractionType.Primary => _primaryKeyLabel,
            InteractionType.Secondary => _secondaryKeyLabel,
            _ => type.ToString()
        };
    }
}