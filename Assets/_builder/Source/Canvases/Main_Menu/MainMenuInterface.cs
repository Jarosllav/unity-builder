using UnityEngine;
using UnityEngine.UI;

namespace nobodyworks.builder
{
    public class MainMenuInterface : InterfaceManager
    {
        [SerializeField]
        private Button _playButton;

        protected override void OnStarted()
        {
            _playButton.onClick.AddListener(PlayButtonClickedHandler);
        }

        private void PlayButtonClickedHandler()
        {
            SessionManager.Instance.CreateSession();
        }
    }
}