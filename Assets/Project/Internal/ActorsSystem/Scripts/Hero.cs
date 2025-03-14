using System.Collections;
using System.Collections.Generic;
using Project.Internal.ActorSystem;
using Project.Internal.Interfaces;
using Project.Internal.SkillsSystem;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Project.Internal.ActorSystem
{
    public class Hero : BaseActor, ISkillsUser
    {
        public BaseActorStats GetAllStats()
        {
            return this.GetActorData<HeroData>().Stats;
        }
    }
}
