using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Internal.SkillsSystem
{
    public static class SkillsRegistry
    {
        public static List<BaseSkill> SkillsDatas = new List<BaseSkill>
        {
            new FistPunchSkill{
                SkillInfo = new SkillInfo("FistPunch", "FistPunch"){
                    Damage = 10.0f,
                    MaxTargets = 3,
                }
            }
        };

        public static BaseSkill GetSkillDataByID(string ID)
        {
            return SkillsDatas.Find(s => s.SkillInfo.ID == ID).Clone();
        }
    }
}
