
using UnityEngine;

namespace Shiroi.FX.Examples {
    public class Ownable<T> : MonoBehaviour where T : Behaviour {
        private T owner;

        public T Owner {
            get {
                return owner;
            }
        }

        public bool RequestOwnership(T newOwner, bool force = false) {
            if (HasOwner) {
                if (force) {
                    RevokeOwnership();
                } else {
                    return false;
                }
            }

            owner = newOwner;
            return true;
        }

        private void RevokeOwnership() {
            owner = null;
        }

        public bool HasOwner {
            get {
                return owner != null;
            }
        }
    }
}