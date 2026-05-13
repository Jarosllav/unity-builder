using System;
using UnityEngine;
using nobodyworks.builder.character;
using nobodyworks.builder.input;

namespace nobodyworks.builder
{
    [DefaultExecutionOrder((int)ExecutionOrder.Default)]
    public class GameManager : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private GameObject _characterPrefab;

        #endregion
        
        private CharacterManager _playerCharacterManager;
        
        public CharacterManager PlayerCharacterManager => _playerCharacterManager;
        
        public event Action OnSetupped;
        
        public void Awake()
        {
            _playerCharacterManager = GetOrCreateCharacter();
        }

        public void Start()
        {
            OnSetupped?.Invoke();
        }

        public void OnDestroy()
        {
            OnSetupped = null;
        }
            
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