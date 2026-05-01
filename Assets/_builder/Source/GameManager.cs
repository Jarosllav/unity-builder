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
            _playerCharacterManager = CreateCharacter(new PlayerInputProvider());
        }

        private CharacterManager CreateCharacter(IInputProvider inputProvider)
        {
            var characterGameObject = GameObject.Instantiate(_characterPrefab);
            var characterManager = characterGameObject.GetComponent<CharacterManager>();
            characterManager.SetInputProvider(inputProvider);
            
            return characterManager;
        }
    }
}