using UnityEngine;
using UnityEngine.UI;

namespace nobodyworks.builder
{
    public class PauseMenuInterface : InterfaceManager
    {
        [SerializeField]
        private Button _exitButton;

        protected override void OnStarted()
        {
            _exitButton.onClick.AddListener(ExitButtonClickedHandler);
        }

        private void ExitButtonClickedHandler()
        {
            SessionManager.Instance.DestroySession();
        }
    }
}