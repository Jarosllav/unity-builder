using System;
using UnityEngine;
using Eflatun.SceneReference;
using nobodyworks.builder.building;
using nobodyworks.builder.items;

namespace nobodyworks.builder
{
    public class SessionContext
    {
        public bool FromMainMenu = false;
        public bool IsNewGame = false;
    }
    
    [DefaultExecutionOrder((int)ExecutionOrder.Early)]
    public class SessionManager : MonoBehaviour
    {
        private static SessionManager _instance;
        public static SessionManager Instance => _instance;
        
        #region Inspector

        [SerializeField]
        private SceneReference _loadingSceneReference;
        
        [SerializeField]
        private SceneReference _menuSceneReference;
        
        [SerializeField]
        private SceneReference _gameplaySceneReference;
        
        [SerializeField]
        private ItemsDatabase _itemsDatabase;
        
        [SerializeField]
        private ElementsDatabase _elementsDatabase;
        
        #endregion
        
        private readonly SessionContext _context = new();
        
        private SceneLoaderController _sceneLoaderController;
        private GameManager _gameManager;
        private bool _isDestroying = false;
        
        public SessionContext Context => _context;
        public GameManager GameManager => _gameManager;

        #region Initialization

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
            
            Databases.Setup(_itemsDatabase, _elementsDatabase);
            
            DontDestroyOnLoad(this);
            
            _sceneLoaderController = new(_loadingSceneReference);
            _sceneLoaderController.OnLoaded += SceneLoadedHandler;
        }

        public void Start()
        {
            if (_isDestroying)
            {
                return;
            }
            
            _sceneLoaderController.CheckActiveScene();
            TryInstallGameManager();
        }

        public void OnDestroy()
        {
            if (_isDestroying)
            {
                return;
            }
            
            _sceneLoaderController.Dispose();
        }

        #endregion
        
        public void CreateSession()
        {
            _context.FromMainMenu = true;
            _context.IsNewGame = true;
            
            _ = _sceneLoaderController.ChangeScene(_gameplaySceneReference);
        }

        public void DestroySession()
        {
            _ = _sceneLoaderController.ChangeScene(_menuSceneReference);
        }

        private void TryInstallGameManager()
        {
            _gameManager = GameObject.FindAnyObjectByType<GameManager>();

            if (_gameManager != null)
            {
                _gameManager.Install(_context);
            }
        }
        
        private void SceneLoadedHandler(SceneReference sceneReference)
        {
            TryInstallGameManager();
        }
    }
}
