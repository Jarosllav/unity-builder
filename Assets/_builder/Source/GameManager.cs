using System;
using UnityEngine;
using nobodyworks.builder.character;
using nobodyworks.builder.clock;
using nobodyworks.builder.input;

namespace nobodyworks.builder
{
    [DefaultExecutionOrder((int)ExecutionOrder.Default)]
    public class GameManager : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private ClockSettings _clockSettings;
        [SerializeField]
        private GameObject _characterPrefab;

        #endregion
        
        private ClockController _clockController;
        private CharacterManager _playerCharacterManager;
        
        public CharacterManager PlayerCharacterManager => _playerCharacterManager;
        public ClockController ClockController => _clockController;
        
        public event Action OnSetupped;
        
        public void Awake()
        {
            _clockController = new(_clockSettings);
            
            _playerCharacterManager = GetOrCreateCharacter();
        }

        public void Start()
        {
            OnSetupped?.Invoke();
        }

        public void OnDestroy()
        {
            _clockController.Dispose();
            
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
#if DEBUG
            var characterManagerInScene = FindAnyObjectByType<CharacterManager>();

            if (characterManagerInScene != null)
            {
                characterManagerInScene.Install();
                
                return  characterManagerInScene;
            }
#endif
            var characterGameObject = GameObject.Instantiate(_characterPrefab);
            var characterManager = characterGameObject.GetComponent<CharacterManager>();

            characterManager.Install();
            
            return characterManager;
        }
    }
}