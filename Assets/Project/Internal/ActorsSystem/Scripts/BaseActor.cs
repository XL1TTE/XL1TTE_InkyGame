using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

namespace Project.Internal.ActorSystem
{
    public abstract class BaseActor : MonoBehaviour
    {
        [SerializeField] public string ActorID;
        [HideInInspector] protected BaseActorData ActorData;
        [SerializeField] public Sprite Avatar;


        public virtual void AttachActorData(BaseActorData actorData)
        {
            ActorData = actorData;
        }

        public virtual ActorDataType GetActorData<ActorDataType>() where ActorDataType : BaseActorData
        {
            return ActorData as ActorDataType;
        }
    }
}
