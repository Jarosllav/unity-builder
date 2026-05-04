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
        
        public void Awake()
        {
            _playerCharacterManager = GetOrCreateCharacter();
        }

        private CharacterManager GetOrCreateCharacter()
        {
#if DEBUG
            var characterManagerInScene = FindAnyObjectByType<CharacterManager>();

            if (characterManagerInScene != null)
            {
                return  characterManagerInScene;
            }
#endif
            var characterGameObject = GameObject.Instantiate(_characterPrefab);
            var characterManager = characterGameObject.GetComponent<CharacterManager>();
            
            return characterManager;
        }
    }
}