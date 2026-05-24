using System;
using IngameDebugConsole;
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
        private static GameManager _instance;
        public static GameManager Instance => _instance;
        
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
        private PlayerInputProvider _playerInputProvider;
        private SessionContext _sessionContext;
        private bool _isInstalled = false;
        private bool _isDestroying = false;
        
        public CharacterManager PlayerCharacterManager => _playerCharacterManager;
        public PlayerInputProvider PlayerInputProvider => _playerInputProvider;
        public ClockController ClockController => _clockController;
        public SkyController SkyController => _skyController;
        public CutscenesController CutscenesController => _cutscenesController;
        
        public event Action OnSetupped;
        
        public void Awake()
        {
            if (_instance != null)
            {
                _isDestroying = true;
                GameObject.Destroy(this);
                return;
            }

            if (_instance == null)
            {
                _instance = this;
            }
            
            _clockController = new(_clockSettings);
            _skyController = new(_skySettings, _clockController);
            _cutscenesController = new(_cutscenesSettings);
            
            _playerCharacterManager = GetOrCreateCharacter();
            _playerInputProvider = _playerCharacterManager.GetComponent<PlayerInputProvider>();
        }

        public void Start()
        {
            if (_isDestroying)
            {
                return;
            }
            
            OnSetupped?.Invoke();
        }

        public void Install(SessionContext context)
        {
            if (_isInstalled || _isDestroying)
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
            if (_isDestroying)
            {
                return;
            }
            
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
        
        [ConsoleMethod("settime", "")]
        public static void Cmd_SetTime(int hours, int minutes)
        {
            Instance.ClockController.SetTime(new TimeReference(TimeUnits.WithoutDay, 0, hours, minutes, 0));
        }
        
        [ConsoleMethod("gettime", "")]
        public static void Cmd_GetTime()
        {
            var time = Instance.ClockController.GetTime();
            Debug.Log($"{time.Hours}:{time.Minutes}");
        }
    }
}