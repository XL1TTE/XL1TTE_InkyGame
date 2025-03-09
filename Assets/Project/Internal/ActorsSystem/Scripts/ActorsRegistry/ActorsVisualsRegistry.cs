using System.Collections;
using System.Collections.Generic;
using Codice.CM.Common.Serialization;
using UnityEngine;

namespace Project.Internal.ActorSystem
{
    public class ActorsVisualsRegistry : MonoBehaviour
    {
        [HideInInspector] public static ActorsVisualsRegistry instance;


        [Header("Heroes")]
        [SerializeField] public List<Hero> Heroes = new();

        [Header("Enemies")]
        [SerializeField] public List<Enemy> Enemies = new();

        public IEnumerator Init()
        {
            Debug.Log("Initializing ActorVisualsRegistry....");
            if (instance == null)
            {
                instance = this;
            }
            yield return null;
        }

        public Hero GetHeroPrefabByID(string ID)
        {
            return Heroes.Find(actor => actor.ActorID == ID);
        }

        public Enemy GetEnemyPrefabByID(string ID)
        {
            return Enemies.Find(e => e.ActorID == ID);
        }

    }
}
