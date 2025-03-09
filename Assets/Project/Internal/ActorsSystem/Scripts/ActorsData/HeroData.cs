using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Internal.ActorSystem
{
    public class HeroData : BaseActorData
    {
        public HeroStats Stats { get; set; }
        public override T Clone<T>()
        {
            var clone = new HeroData();
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
