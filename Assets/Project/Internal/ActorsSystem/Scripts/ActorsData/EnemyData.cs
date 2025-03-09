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
        public override T Clone<T>()
        {
            var clone = new EnemyData();
            clone.ActorID = this.ActorID;
            clone.ActorName = this.ActorName;
            clone.Stats = this.Stats;
            return clone as T;
        }


        public override string GetAllStatsInString()
        {
            var stats = $"Health: {Stats.Health}\nPhysical Damage: {Stats.PhysicalDamage}\nAttributes:\nStrenght: {Stats.Attributes.Strenght}\nDexterity: {Stats.Attributes.Dexterity}\nIntelligence{Stats.Attributes.Intelligence}";
            return stats;
        }

    }
}
