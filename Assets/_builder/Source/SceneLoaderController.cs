using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;
using nobodyworks.builder.extensions;

namespace nobodyworks.builder
{
    public class SceneLoaderController
    {
        private readonly SceneReference _loadingSceneReference;
        
        private SceneReference _lastLoadedSceneReference;
        private bool _isLoading = false;
        
        private Scene _loadingScene;
        private Scene _networkLoadedScene;
        
        public bool IsLoading => _isLoading;
        
        public event Action<SceneReference> OnLoaded; 
        
        #region Initialization

        public SceneLoaderController(SceneReference loadingSceneReference)
        {
            _loadingSceneReference = loadingSceneReference;
            
            _ = PrepareLoadingScene();
        }

        public void Dispose()
        {
            OnLoaded = null;
        }

        #endregion

        public void CheckActiveScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            var sceneReference = SceneReference.FromScenePath(activeScene.path);

            _lastLoadedSceneReference = sceneReference;
        }

        public async Task ChangeScene(SceneReference sceneReference, SceneReference fromSceneReference = null)
        {
            await BeginChangeScene(fromSceneReference);
            await LoadSceneLocal(sceneReference);
            await EndChangeScene(sceneReference);
        }

        private async Task BeginChangeScene(SceneReference fromSceneReference = null)
        {
            if (fromSceneReference == null)
            {
                fromSceneReference = _lastLoadedSceneReference;
            }

            _isLoading = true;
            _loadingScene.SetActive(true);

            await Task.Yield();

            if (fromSceneReference != null)
            {
                await SceneManager.UnloadSceneAsync(fromSceneReference.Name, UnloadSceneOptions.None);

                await Task.Yield();
            }
        }

        private async Task EndChangeScene(SceneReference sceneReference)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneReference.Name));
            
            await Task.Delay(1000);
            await Task.Yield();

            _lastLoadedSceneReference = sceneReference;
            _isLoading = false;

            await Task.Yield();

            OnLoaded?.Invoke(sceneReference);
            
            _loadingScene.SetActive(false);
        }
        
        private async Task LoadSceneLocal(SceneReference sceneReference)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneReference.Name, LoadSceneMode.Additive);

            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress < 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                
                await Task.Yield();
            }

            await Task.Yield();
        }
        
        private async Task PrepareLoadingScene()
        {
            await LoadSceneLocal(_loadingSceneReference);
            
            _loadingScene = SceneManager.GetSceneByName(_loadingSceneReference.Name);
            _loadingScene.SetActive(false);
        }
    }
}