using System;
using System.Collections;
using System.Collections.Generic;
using Project.Internal.ActorSystem;
using UnityEngine;

namespace Project.Internal.ActorSystem
{
    public class EnemyData : BaseActorData
    {
        public BaseActorStats Stats { get; set; }
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
            string Health_color_Hex = "#5d1818";
            string Attributes_color_Hex = "#27477f";
            string Damage_color_Hex = "#a8863d";

            var stats = $"<color={Health_color_Hex}>Health: {Stats.Health}</color>\n" +
                        $"<color={Damage_color_Hex}>Damage:</color>\n" +
                        $"<color={Damage_color_Hex}> Physical - {Stats.DamageStats.PhysicalDamage}</color>\n" +
                        $"<color={Attributes_color_Hex}>Attributes:</color>\n" +
                        $"<color={Attributes_color_Hex}> Strength: {Stats.Attributes.Strenght}</color>\n" +
                        $"<color={Attributes_color_Hex}> Dexterity: {Stats.Attributes.Dexterity}</color>\n" +
                        $"<color={Attributes_color_Hex}> Intelligence: {Stats.Attributes.Intelligence}</color>";
            return stats;
        }

    }
}
