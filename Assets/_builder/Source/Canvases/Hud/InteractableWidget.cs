using System;
using nobodyworks.builder.carrying;
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
        private CarrierController _carrierController;
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

            if (_progressImage.fillAmount >= 1f)
            {
                _isHolding = false;
                HideProgress();
            }
        }
        
        public void Setup(InteractionController interactionController, CarrierController carrierController, 
            PlayerInputProvider playerInputProvider)
        {
            _interactionController = interactionController;
            _carrierController = carrierController;
            
            interactionController.OnSelected += InteractionSelectedHandler;
            interactionController.OnDeselected += InteractionDeselectedHandler;
            
            playerInputProvider.OnInteractionStarted += InteractionStartedHandler;
            playerInputProvider.OnInteractionCanceled += InteractionCanceledHandler;
        }

        private void ShowProgress()
        {
            _progressImage.gameObject.SetActive(true);
        }

        private void HideProgress()
        {
            _progressImage.gameObject.SetActive(false);
            ResetProgress();
        }

        private void ShowLabel()
        {
            _interactableLabel.gameObject.SetActive(true);
        }

        private void HideLabel()
        {
            _interactableLabel.gameObject.SetActive(false);
            ResetLabel();
        }
        
        private void Show()
        {
            ShowProgress();
            ShowLabel();
        }

        private void Hide()
        {
            HideProgress();
            HideLabel();
        }

        private void Reset()
        {
            ResetProgress();
            ResetLabel();
        }

        private void ResetProgress()
        {
            _progressImage.fillAmount = 0f;
        }

        private void ResetLabel()
        {
            _interactableLabel.text = string.Empty;
        }

        private void InteractionSelectedHandler(InteractionController _, InteractableManager interactableManager)
        {
            _interactableLabel.text = interactableManager.GetName();
            ShowLabel();
        }
        
        private void InteractionDeselectedHandler(InteractionController _, InteractableManager interactableManager)
        {
            HideLabel();
        }

        private void InteractionStartedHandler(InteractionType interactionType, float duration)
        {
            if (!HasValidInteractable(interactionType) && !HasValidCarryable(interactionType))
            {
                return;
            }
            
            _duration = duration;
            _startTime = Time.time;
            _isHolding = true;
            
            ShowProgress();
        }

        private void InteractionCanceledHandler(InteractionType _)
        {
            HideProgress();
            _isHolding = false;
        }

        private bool HasValidInteractable(InteractionType interactionType)
        {
            var interactableManager = _interactionController.GetCurrentInteractableManager();
            
            return interactableManager != null && 
                   interactableManager.InteractionDefinition.InteractionTypes.HasFlag(interactionType);
        }

        private bool HasValidCarryable(InteractionType interactionType)
        {
            return interactionType == InteractionType.Secondary && _carrierController.IsCarrying;
        }
    }
}