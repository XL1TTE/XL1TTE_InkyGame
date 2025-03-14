using System.Collections;
using System.Collections.Generic;
using Project.Internal.ActorSystem;
using Project.Internal.SkillsSystem;
using UnityEngine;

namespace Project.Internal.Interfaces
{
    public interface ISkillsUser
    {
        public BaseActorStats GetAllStats();
    }
}
