using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

        protected abstract string GetAnimatorTrigger();

        public abstract IEnumerator Execute(List<IDamagable> targets, ISkillsUser skillUser);

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

        public override IEnumerator Execute(List<IDamagable> targets, ISkillsUser skillUser)
        {
            if (targets.Count > 0)
            {
                var stats = skillUser.GetAllStats();

                var user_damage = stats.DamageStats.PhysicalDamage;

                var user_animator = skillUser.GetAnimator();

                var user_tranform = skillUser.GetTransform();

                var user_origin_position = user_tranform.position;

                foreach (var target in targets)
                {

                    var target_position = target.GetPosition() - new Vector3(1.5f, 0f, 0f);
                    user_tranform.DOMove(target_position, 0.25f);

                    user_animator.SetTrigger(GetAnimatorTrigger());


                    yield return new WaitUntil(() => user_animator.GetBool("IsAttackOver"));

                    target.GetDamage(SkillInfo.Damage + user_damage);


                    yield return new WaitForSeconds(1f);
                    user_tranform.DOMove(user_origin_position, 0.25f);
                }

            }
            yield return null;
        }

        protected override string GetAnimatorTrigger()
        {
            return "FistAttack";
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

        public override IEnumerator Execute(List<IDamagable> targets, ISkillsUser skillUser)
        {
            if (targets.Count > 0)
            {
                var stats = skillUser.GetAllStats();
                var user_damage = stats.DamageStats.PhysicalDamage;
                var user_animator = skillUser.GetAnimator();

                var targetsCopy = new List<IDamagable>(targets);

                foreach (var target in targetsCopy)
                {

                    user_animator.SetTrigger(GetAnimatorTrigger());


                    yield return new WaitUntil(() => user_animator.GetBool("IsAttackOver"));

                    target.GetDamage(SkillInfo.Damage + user_damage);

                    yield return new WaitForSeconds(1f);

                }
            }
        }


        protected override string GetAnimatorTrigger()
        {
            return "BowAttack";
        }
    }
}
