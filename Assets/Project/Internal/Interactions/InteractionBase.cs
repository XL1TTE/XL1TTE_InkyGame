using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Internal.Interactions
{

    public enum Priorities
    {
        very_low = 0,
        low = 1,
        standart = 2,
        high = 3,
        very_high = 4,

    }
    public abstract class InteractionBase
    {
        public virtual Priorities Priority()
        {
            return Priorities.standart;
        }
    }
}
