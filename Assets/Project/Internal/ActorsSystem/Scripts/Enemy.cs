using System.Collections;
using System.Collections.Generic;
using Project.Internal.ActorSystem;
using Project.Internal.Interfaces;
using UnityEngine;

namespace Project.Internal.ActorSystem
{
    public class Enemy : BaseActor, IDamagable
    {
        public void GetDamage(float damage)
        {
            this.GetActorData<EnemyData>().Stats.Health -= damage;
        }
    }
}
