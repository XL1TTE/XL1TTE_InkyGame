using System.Collections;
using System.Collections.Generic;
using Project.Internal.ActorSystem;
using Project.Internal.SkillsSystem;
using UnityEditor.Animations;
using UnityEngine;

namespace Project.Internal.Interfaces
{
    public interface ISkillsUser
    {
        public Animator GetAnimator();
        public BaseActorStats GetAllStats();


        public Transform GetTransform();
    }
}
