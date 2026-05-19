using System;
using nobodyworks.builder.input;
using nobodyworks.builder.interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nobodyworks.builder.interfaces
{
    public class InteractableWidget : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private Image _progressImage;
        
        [SerializeField]
        private TMP_Text _interactableLabel;

        #endregion

        private InteractionController _interactionController;
        private float _startTime;
        private float _duration;
        private bool _isHolding;

        public void Awake()
        {
            Hide();
        }

        public void Update()
        {
            if (!_isHolding)
            {
                return;
            }
            
            _progressImage.fillAmount = Mathf.Clamp01((Time.time - _startTime) / _duration);
        }

        public void Setup(InteractionController interactionController, PlayerInputProvider playerInputProvider)
        {
            _interactionController = interactionController;
            interactionController.OnSelected += InteractionSelectedHandler;
            interactionController.OnDeselected += InteractionDeselectedHandler;
            
            playerInputProvider.OnInteractionStarted += InteractionStartedHandler;
            playerInputProvider.OnInteractionCanceled += InteractionCanceledHandler;
        }

        private void Show()
        {
            Reset();
            
            _progressImage.gameObject.SetActive(true);
            _interactableLabel.gameObject.SetActive(true);
        }

        private void Hide()
        {
            _progressImage.gameObject.SetActive(false);
            _interactableLabel.gameObject.SetActive(false);
        }

        private void Reset()
        {
            ResetProgress();
            _interactableLabel.text = string.Empty;
        }

        private void ResetProgress()
        {
            _progressImage.fillAmount = 0f;
        }

        private void InteractionSelectedHandler(InteractionController _, InteractableManager interactableManager)
        {
            Show();
            
            _interactableLabel.text = interactableManager.GetName();
        }
        
        private void InteractionDeselectedHandler(InteractionController _, InteractableManager interactableManager)
        {
            Hide();
        }

        private void InteractionStartedHandler(InteractionType interactionType, float duration)
        {
            var interactableManager = _interactionController.GetCurrentInteractableManager();
            
            if (interactableManager == null 
                || !interactableManager.InteractionDefinition.InteractionTypes.HasFlag(interactionType))
            {
                return;
            }
            
            _duration = duration;
            _startTime = Time.time;
            _isHolding = true;
        }

        private void InteractionCanceledHandler(InteractionType _)
        {
            ResetProgress();
            _isHolding = false;
        }
    }
}