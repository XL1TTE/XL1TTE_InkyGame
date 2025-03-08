using System.Collections;
using System.Collections.Generic;
using Project.Internal.ActorSystem;
using Project.Internal.Interactions;
using Project.Internal.UI;
using Project.Internal.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Internal.BattleSystem
{


    public class TestEnemiesSpawnInteraction : InteractionBase, IOnBattleInitStart
    {
        public override Priorities Priority()
        {
            return Priorities.very_low;
        }
        public IEnumerator OnInitStart(BattleManager context)
        {
            // SPAWNING ENEMIES
            foreach (var slot in context.EnemiesSlots)
            {
                Object.Instantiate(context.EnemyPrefab, slot);
                yield return new WaitForSeconds(1);
            }

        }
    }
    public class TestHeroesSpawnInteraction : InteractionBase, IOnBattleInitStart
    {
        public override Priorities Priority()
        {
            return Priorities.low;
        }
        public IEnumerator OnInitStart(BattleManager context)
        {
            for (int i = 0; i < context.HeroesToSpawn.Count; i++)
            {

                if (ActorFactory.TryBuildHero(context.HeroesToSpawn[i], true, out var hero))
                {
                    var hero_info = hero.GetActorData<HeroData>();
                    Debug.Log($"Spawning {hero.ActorID}... His name is {hero_info.ActorName}\n{hero_info.GetAllStatsInString()}");
                    yield return new WaitForSeconds(1);
                    SpawnerUtility.SpawnItemIn(hero.gameObject, context.HeroesSlots[i], true);
                }
            }

            yield return null;
        }
    }


    public class BattleReadyInteraction : InteractionBase, IOnBattleReady
    {
        public IEnumerator OnBattleReady(BattleManager context)
        {
            context.PreFightAnimationObject.SetActive(true);
            yield return new WaitForSeconds(3);

            context.PreFightAnimationObject.SetActive(false);


            Debug.LogWarning("Don't forget to rework the ui skills setup.");
            context.Skills.Init();

            context.StartCoroutine(SelectableNavigationUtility.SetupRowsLikeNavigation(context.Skills._selectables, 7));

            context.Skills.EnableBehaviour();
        }
    }

    public class BattleManager : MonoBehaviour
    {
        [SerializeField] public Camera MainBattleCamera;
        [SerializeField] public GameObject PreFightAnimationObject;


        [Header("Enemies setup")]

        [SerializeField] public GameObject EnemyPrefab;
        [SerializeField] public List<Transform> EnemiesSlots;

        [Header("Heroes setup")]
        [SerializeField] public List<Transform> HeroesSlots;
        [SerializeField] public List<string> HeroesToSpawn = new List<string>(3);


        [Header("TO REMOVE LATER")]
        [SerializeField] public DynamicSelectablesEventHandler Skills;

        IEnumerator Start()
        {
            InteractionManager interactions = new InteractionManager();
            interactions.Init();
            foreach (var interaction in interactions.FindAllOf<IOnBattleInitStart>())
            {
                yield return interaction.OnInitStart(this);
            }

            foreach (var interaction in interactions.FindAllOf<IOnBattleReady>())
            {
                yield return interaction.OnBattleReady(this);
            }
        }
    }
}
