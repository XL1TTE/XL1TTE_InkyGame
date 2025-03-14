using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Internal.ActorSystem
{

    [Serializable]
    public class ActorAttributes
    {
        public float Strenght { get; set; } = 10.0f;
        public float Intelligence { get; set; } = 10.0f;
        public float Dexterity { get; set; } = 10.0f;
    }

    [Serializable]
    public class DamageStats
    {
        public float PhysicalDamage = 1.0f;
    }

    [Serializable]
    public class BaseActorStats
    {
        public float Health { get; set; } = 100.0f;

        public ActorAttributes Attributes { get; set; }

        public DamageStats DamageStats { get; set; }
    }
}
