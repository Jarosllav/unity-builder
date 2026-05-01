using System.Collections.Generic;
using UnityEngine;

namespace nobodyworks
{
    public abstract class MovementDriver
    {
        public virtual void ApplyMotion(Vector3 motion) { }
        public virtual void DisableMotion() { }
        public virtual void EnableMotion() { }

    }
}
