using System;
using UnityEngine;
using Eflatun.SceneReference;
using nobodyworks.builder.items;

namespace nobodyworks.builder
{
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
        
        #endregion
        
        private SceneLoaderController _sceneLoaderController;
        
        private bool _isDestroying = false;

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
            
            Databases.Setup(_itemsDatabase);
            
            DontDestroyOnLoad(this);
            
            _sceneLoaderController = new(_loadingSceneReference);
        }

        public void Start()
        {
            if (_isDestroying)
            {
                return;
            }
            
            _sceneLoaderController.CheckActiveScene();
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
            _ = _sceneLoaderController.ChangeScene(_gameplaySceneReference);
        }

        public void DestroySession()
        {
            _ = _sceneLoaderController.ChangeScene(_menuSceneReference);
        }
    }
}
