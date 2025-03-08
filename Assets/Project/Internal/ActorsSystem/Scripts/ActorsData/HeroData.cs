using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Internal.ActorSystem
{
    public class HeroData : BaseActorData
    {
        public HeroStats Stats { get; set; }
        public override HeroData Clone<HeroData>()
        {
            return Activator.CreateInstance(this.GetType()) as HeroData;
        }


        public override string GetAllStatsInString()
        {
            var stats = $"Health: {Stats.Health}\nPhysical Damage: {Stats.PhysicalDamage}\nAttributes:\nStrenght: {Stats.Attributes.Strenght}\nDexterity: {Stats.Attributes.Dexterity}\nIntelligence{Stats.Attributes.Intelligence}";
            return stats;
        }
    }
}
