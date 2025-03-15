using System.Collections;
using System.Collections.Generic;
using Project.Internal.ActorSystem;
using Project.Internal.Interfaces;
using Project.Internal.SkillsSystem;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Animations;
using UnityEngine;

namespace Project.Internal.ActorSystem
{
    public class Hero : BaseActor, ISkillsUser
    {
        [SerializeField] Animator Animator;


        public BaseActorStats GetAllStats()
        {
            return this.GetActorData<HeroData>().Stats;
        }

        public Animator GetAnimator()
        {
            return Animator;
        }

        public Transform GetTransform()
        {
            return gameObject.transform;
        }

        public void OnAttackStateChanged()
        {
            var current = Animator.GetBool("IsAttackOver");
            Animator.SetBool("IsAttackOver", !current);
        }
    }
}
