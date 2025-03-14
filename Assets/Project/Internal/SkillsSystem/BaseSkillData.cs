using System.Collections;
using System.Collections.Generic;
using Project.Internal.ActorSystem;
using Project.Internal.Interfaces;
using UnityEngine;

namespace Project.Internal.SkillsSystem
{

    public class SkillInfo
    {
        public SkillInfo(string ID, string Visuals_ID)
        {
            this.ID = ID;
            this.SkillVisualID = Visuals_ID;
        }
        public string ID;
        public string SkillVisualID;
        public int Level = 1;

        public float Damage = 1.0f;

        public int MaxTargets = 1;
    }

    public abstract class BaseSkill
    {
        public SkillInfo SkillInfo;

        public abstract void Execute(List<IDamagable> targets, ISkillsUser skillUser);

        public abstract BaseSkill Clone();

    }

    public class FistPunchSkill : BaseSkill
    {
        public override BaseSkill Clone()
        {
            var clone = new FistPunchSkill();
            clone.SkillInfo = this.SkillInfo;
            return clone;
        }

        public override void Execute(List<IDamagable> targets, ISkillsUser skillUser)
        {
            if (targets.Count > 0)
            {
                var stats = skillUser.GetAllStats();

                var user_damage = stats.DamageStats.PhysicalDamage;

                foreach (var target in targets)
                    target.GetDamage(SkillInfo.Damage + user_damage);
            }
        }
    }

    public class BowFireSKill : BaseSkill
    {
        public override BaseSkill Clone()
        {
            var clone = new BowFireSKill();
            clone.SkillInfo = this.SkillInfo;
            return clone;
        }

        public override void Execute(List<IDamagable> targets, ISkillsUser skillUser)
        {
            if (targets.Count > 0)
            {
                var stats = skillUser.GetAllStats();

                var user_damage = stats.DamageStats.PhysicalDamage;

                foreach (var target in targets)
                    target.GetDamage(SkillInfo.Damage + user_damage);
            }
        }

    }
}
