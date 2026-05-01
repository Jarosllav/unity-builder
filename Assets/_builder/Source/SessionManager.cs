using System;
using Eflatun.SceneReference;
using UnityEngine;

namespace nobodyworks.builder
{
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
