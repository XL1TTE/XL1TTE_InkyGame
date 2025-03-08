using System;
using System.Collections;
using System.Collections.Generic;
using Project.Internal.ActorSystem;
using UnityEngine;

namespace Project.Internal.ActorSystem
{
    public class EnemyData : BaseActorData
    {
        public EnemyStats Stats { get; set; }
        public override EnemyData Clone<EnemyData>()
        {
            return Activator.CreateInstance(this.GetType()) as EnemyData;
        }
    }
}
