using TMPro;
using UnityEngine;

namespace nobodyworks.builder.interfaces
{
    public class HudInterfaceManager : InterfaceManager
    {
        [SerializeField]
        private QuickBarWidget _quickBarWidget;

        [SerializeField]
        private KeyHintsWidget _keyHintsWidget;

        [SerializeField]
        private InteractableWidget _interactableWidget;

        [SerializeField]
        private TMP_Text _dayTimeLabel;

        [SerializeField]
        private TMP_Text _dayPhaseLabel;

        protected override void OnInitialized()
        {
            _quickBarWidget.Setup(CharacterManager.QuickBarController);
            _keyHintsWidget.Setup(CharacterManager.InteractionController);
            _interactableWidget.Setup(CharacterManager.InteractionController, CharacterManager.CarrierController,
                GameManager.PlayerInputProvider);
        }

        public void Update()
        {
            _dayTimeLabel.text = $"{GameManager.ClockController.Hour:D2}:{GameManager.ClockController.Minute:D2}";
            _dayPhaseLabel.text = $"{GameManager.ClockController.Phase}";
        }
    }
}
