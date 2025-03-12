using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Internal.SkillsSystem
{

    [Serializable]
    public class SkillVisual : MonoBehaviour
    {
        [SerializeField] public string ID;
        [SerializeField] public Sprite SkillIcon;
        protected BaseSkill Skill;
        public BaseSkill GetSkill()
        {
            return Skill;
        }
    }
}
