using System;
using System.Collections.Generic;
using UnityEngine;

namespace nobodyworks.builder.utilities
{
    public class Pool<T> : IDisposable 
        where T : Component
    {
        private readonly Func<T> _factory;
        private readonly Queue<T> _inactive = new Queue<T>();
        private readonly List<T> _active = new List<T>();

        public int ActiveCount => _active.Count;
        public int InactiveCount => _inactive.Count;

        public Pool(Func<T> factory, int prewarm = 0)
        {
            _factory = factory;

            for (var i = 0; i < prewarm; i++)
            {
                Enqueue(Create());
            }
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            var instance = _inactive.Count > 0 ? _inactive.Dequeue() : Create();
            instance.transform.SetPositionAndRotation(position, rotation);
            instance.gameObject.SetActive(true);
            _active.Add(instance);
            return instance;
        }

        public T Get()
        {
            return Get(Vector3.zero, Quaternion.identity);
        }

        public void Return(T instance)
        {
            if (!_active.Remove(instance))
            {
                return;
            }

            Enqueue(instance);
        }

        public void Dispose()
        {
            ReturnAll();
            _inactive.Clear();
        }

        private T Create()
        {
            var instance = _factory();
            instance.gameObject.SetActive(false);
            return instance;
        }

        private void Enqueue(T instance)
        {
            instance.gameObject.SetActive(false);
            _inactive.Enqueue(instance);
        }
        
        private void ReturnAll()
        {
            for (var i = _active.Count - 1; i >= 0; i--)
            {
                Enqueue(_active[i]);
            }

            _active.Clear();
        }
    }
}
