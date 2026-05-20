using nobodyworks.builder.extensions;
using TriInspector;
using UnityEngine;
using UnityEngine.Localization;

namespace nobodyworks.builder.interaction
{
    [CreateAssetMenu(menuName = "Game/Interactions/Interaction Definition", fileName = "Interaction_Definition")]
    public class InteractionDefinition : ScriptableObject
    {
        [SerializeField]
        private InteractionType _interactionTypes = InteractionType.None;
        
        [SerializeField, ShowIf(nameof(HasPrimaryType))]
        private LocalizedString _primaryLabel;
        
        [SerializeField, ShowIf(nameof(HasSecondaryType))]
        private LocalizedString _secondaryLabel;
        
        private bool HasPrimaryType => _interactionTypes.HasFlag(InteractionType.Primary);
        private bool HasSecondaryType => _interactionTypes.HasFlag(InteractionType.Secondary);
        
        public InteractionType InteractionTypes => _interactionTypes;
        public string PrimaryLabel => _primaryLabel.GetText();
        public string SecondaryLabel => _secondaryLabel.GetText();

        public string GetLabel(InteractionType interactionType)
        {
            var label = interactionType switch
            {
                InteractionType.Primary => PrimaryLabel,
                InteractionType.Secondary => SecondaryLabel,
                _ => string.Empty
            };

#if DEBUG
            if (label == string.Empty)
            {
                label = interactionType.ToString();
            }
#endif
            
            return label;
        }
    }
}