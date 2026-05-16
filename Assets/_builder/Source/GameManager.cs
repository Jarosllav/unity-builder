using System;
using UnityEngine;
using nobodyworks.builder.character;
using nobodyworks.builder.clock;
using nobodyworks.builder.cutscene;
using nobodyworks.builder.input;
using nobodyworks.builder.sky;
using TriInspector;

namespace nobodyworks.builder
{
    [DefaultExecutionOrder((int)ExecutionOrder.Default)]
    public class GameManager : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private ClockSettings _clockSettings;
        
        [SerializeField]
        private SkySettings _skySettings;
        
        [SerializeField]
        private CutscenesSettings _cutscenesSettings;
        
        [SerializeField]
        private GameObject _characterPrefab;

        #endregion
        
        private ClockController _clockController;
        private SkyController _skyController;
        private CutscenesController _cutscenesController;
        private CharacterManager _playerCharacterManager;
        private SessionContext _sessionContext;
        private bool _isInstalled = false;
        
        public CharacterManager PlayerCharacterManager => _playerCharacterManager;
        public ClockController ClockController => _clockController;
        public SkyController SkyController => _skyController;
        public CutscenesController CutscenesController => _cutscenesController;
        
        public event Action OnSetupped;
        
        public void Awake()
        {
            _clockController = new(_clockSettings);
            _skyController = new(_skySettings, _clockController);
            _cutscenesController = new(_cutscenesSettings);
            
            _playerCharacterManager = GetOrCreateCharacter();
        }

        public void Start()
        {
            OnSetupped?.Invoke();
        }

        public void Install(SessionContext context)
        {
            if (_isInstalled)
            {
                return;
            }
            
            _sessionContext = context;
            _isInstalled = true;
            
            _skyController.Refresh();
            
            if (_sessionContext.FromMainMenu && _sessionContext.IsNewGame)
            {
                SetupNewGame();
            }
        }

        public void OnDestroy()
        {
            _clockController.Dispose();
            _skyController.Dispose();
            
            OnSetupped = null;
        }

        public void Update()
        {
            var deltaTime = Time.deltaTime;
            
            _clockController.Tick(deltaTime);
        }

        public void FixedUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;
        }

        private CharacterManager GetOrCreateCharacter()
        {
            var characterManagerInScene = FindAnyObjectByType<CharacterManager>();

            if (characterManagerInScene != null)
            {
                characterManagerInScene.Install();
                
                return  characterManagerInScene;
            }
            
            var characterGameObject = GameObject.Instantiate(_characterPrefab);
            var characterManager = characterGameObject.GetComponent<CharacterManager>();

            characterManager.Install();
            
            return characterManager;
        }

        private void SetupNewGame()
        {
            _cutscenesController.Play<CarriageCutsceneManager>();
        }

#if UNITY_EDITOR
        [Button, ShowInPlayMode]
        [Button("Day"), ShowInPlayMode]
        protected void Editor_Day()
        {
            _clockController.SetTime(_clockSettings.DayTimeReference);
        }

        [Button("Night"), ShowInPlayMode]
        protected void Editor_Night()
        {
            _clockController.SetTime(_clockSettings.NightTimeReference);
        }

        [Button("Play carriage cutscene"), ShowInPlayMode]
        protected void Editor_PlayCarriageCutscene()
        {
            _cutscenesController.Play<CarriageCutsceneManager>();
        }
        
#endif
    }
}