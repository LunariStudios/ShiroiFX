using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtilities;

namespace Shiroi.FX.Utilities {
    public abstract class ObjectPool<T> : MonoBehaviour where T : Component {
        public ushort Prewarm = 20;
        public ushort AllowedTemporaryObjects = 5;
        public T Prefab;

        private void Start() {
            for (ushort i = 0; i < Prewarm; i++) {
                idle.Add(CreateNew(false, true));
            }
        }

        private T CreateNew(bool active, bool attachToTransform) {
            var obj = Instantiate(Prefab);
            obj.gameObject.SetActive(active);
            if (attachToTransform) {
                obj.transform.parent = transform;
            }

            return obj;
        }

        [SerializeField, HideInInspector]
        private List<T> idle = new List<T>();

        [SerializeField, HideInInspector]
        private List<T> allocatedTemporarily = new List<T>();

        [SerializeField, HideInInspector]
        private List<T> inUse = new List<T>();

        public void Return(T obj) {
            if (allocatedTemporarily.Contains(obj)) {
                allocatedTemporarily.Remove(obj);
                Destroy(obj);
                return;
            }

            if (!inUse.Contains(obj)) {
                return;
            }
            obj.gameObject.SetActive(false);
            obj.transform.parent = transform;
            inUse.Remove(obj);
            idle.Add(obj);
        }

        public T Get() {
            return idle.IsEmpty() ? GetTemporary() : GetIdle();
        }

        private T GetIdle() {
            var obj = idle.First();
            obj.gameObject.SetActive(true);
            obj.gameObject.transform.parent = null;
            idle.Remove(obj);
            inUse.Add(obj);
            return obj;
        }

        private T GetTemporary() {
            if (allocatedTemporarily.Count >= AllowedTemporaryObjects) {
                Debug.LogWarning(string.Format("There are too many temporary objects created by this pool! (Max: {0})", AllowedTemporaryObjects));
                return null;
            }

            var temp = CreateNew(true, false);
            allocatedTemporarily.Add(temp);
            return temp;
        }
    }
}