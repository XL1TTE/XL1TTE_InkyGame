using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Internal.Interfaces
{
    public interface IDamagable
    {
        public void GetDamage(float damage);

        public Vector3 GetPosition();
    }
}
