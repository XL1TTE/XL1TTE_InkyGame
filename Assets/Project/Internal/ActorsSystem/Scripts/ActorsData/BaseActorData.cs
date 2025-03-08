using System.Collections;
using System.Collections.Generic;
using Codice.CM.Common.Tree;
using UnityEngine;

namespace Project.Internal.ActorSystem
{
    public abstract class BaseActorData
    {
        public string ActorID { get; set; }

        public string ActorName { get; set; }

        public abstract Type Clone<Type>() where Type : BaseActorData;

        public virtual string GetAllStatsInString()
        {
            return "";
        }

    }
}
