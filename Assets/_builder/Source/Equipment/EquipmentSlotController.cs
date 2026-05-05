using System;
using nobodyworks.builder.extensions;
using nobodyworks.builder.items;
using nobodyworks.builder.skeleton;
using nobodyworks.builder.utilities;
using UnityEngine;

namespace nobodyworks.builder.equipment
{
    public class EquipmentSlotController
    {
        private readonly EquipmentSlotDefinition _definition;
        private readonly BoneReference _boneReference;
        
        private Item _item;
        private GameObject _gameObject;

        public EquipmentSlotDefinition Definition => _definition;
        public BoneReference BoneReference => _boneReference;
        public Item Item => _item;
        public GameObject GameObject => _gameObject;

        public EquipmentSlotController(EquipmentSlotDefinition definition, BoneReference boneReference)
        {
            _definition = definition;
            _boneReference = boneReference;
        }

        public bool Equip(Item item)
        {
            if (_item != null)
            {
                // ...
            }

            _item = item;
            CreateGameObject(_item.Definition.Prefab);
            return true;
        }

        public bool Unequip(Item item)
        {
            if (item == null || item != _item)
            {
                return false;
            }

            _item = null;
            DestroyGameObject();
            return true;
        }
        
        private void CreateGameObject(GameObject prefab)
        {
            var boneTransform = _boneReference.Transform;
            _gameObject = GameObject.Instantiate(prefab, boneTransform);
            _gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            var rigidbody = _gameObject.GetComponentInChildren<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
            }

            // TODO(PO): _gameObject.ChangeChildrenLayer(LayerMask.NameToLayer("Disabled"));
            //_gameObject.transform.SetOffset(offset);
        }
        
        private void DestroyGameObject()
        {
            if (_gameObject != null)
            {
                foreach (var collider in _gameObject.GetComponentsInChildren<Collider>())
                {
                    collider.enabled = false;
                    collider.isTrigger = true;
                }

                GameObject.Destroy(_gameObject);
                _gameObject = null;
            }
        }
    }
}
