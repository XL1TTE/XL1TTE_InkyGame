using System.Collections;
using System.Collections.Generic;
using Project.Internal.ActorSystem;
using Project.Internal.BattleSystem.States;
using Project.Internal.Interactions;
using Project.Internal.UI;
using Project.Internal.Utilities;
using UnityEngine;

namespace Project.Internal.BattleSystem
{

    public class StatesInitialization : InteractionBase, IOnBattleInitStart
    {
        public override Priorities Priority()
        {
            return Priorities.very_low;
        }

        public IEnumerator OnInitStart(BattleManager context)
        {
            // States initialization
            yield return BattleStatesManager.Init();
            BattleStatesManager.SetCurrentState<PlayerTurnBattleState>();
        }
    }

    public class EnemiesInitInteraction : InteractionBase, IOnBattleInitStart
    {
        public override Priorities Priority()
        {
            return Priorities.high;
        }
        public IEnumerator OnInitStart(BattleManager context)
        {

            for (int i = 0; i < context.EnemiesSlots.Count; i++)
            {
                var enemy_to_spawn = context.EnemiesToSpawn[i];

                if (ActorFactory.TryBuildEnemy(enemy_to_spawn.ActorDataID, enemy_to_spawn.ActorVisualID, true, out var enemy))
                {
                    var enemy_data = enemy.GetActorData<EnemyData>();
                    Debug.Log($"Spawning {enemy.ActorID}... His name is {enemy_data.ActorName}\n{enemy_data.GetAllStatsInString()}");


                    context.EnemiesEventHandler.AddSubject(enemy);

                    context.EnemiesInBattle.Add(enemy);
                    yield return null;
                }
            }

            context.EnemiesEventHandler.Apply();
        }
    }
    public class HeroesInitInteraction : InteractionBase, IOnBattleInitStart
    {
        public override Priorities Priority()
        {
            return Priorities.high;
        }
        public IEnumerator OnInitStart(BattleManager context)
        {
            for (int i = 0; i < context.HeroesToSpawn.Count; i++)
            {
                var actor_spawn_info = context.HeroesToSpawn[i];
                if (ActorFactory.TryBuildHero(actor_spawn_info.ActorDataID, actor_spawn_info.ActorVisualID, true, out var hero))
                {
                    var hero_info = hero.GetActorData<HeroData>();
                    Debug.Log($"Initialize {hero.ActorID}... His name is {hero_info.ActorName}\n{hero_info.GetAllStatsInString()}");

                    context.HeroesInBattle.Add(hero);
                    yield return null;
                }
            }

            yield return null;
        }
    }


    public class BattleReadyInteraction : InteractionBase, IOnBattleReady
    {
        public IEnumerator OnBattleReady(BattleManager context)
        {

            // Spawning Heroes
            var slot_i = 0;
            foreach (var hero in context.HeroesInBattle)
            {
                SpawnerUtility.SpawnItemIn(hero.gameObject, context.HeroesSlots[slot_i], true);
                slot_i++;
                yield return new WaitForSeconds(1);
            }

            // Spawning Enemies
            slot_i = 0;
            foreach (var enemy in context.EnemiesInBattle)
            {
                SpawnerUtility.SpawnItemIn(enemy.gameObject, context.EnemiesSlots[slot_i], true);
                slot_i++;
                yield return new WaitForSeconds(1);
            }


            // Play preFightAnimation
            context.PreFightAnimationObject.SetActive(true);

            yield return new WaitForSeconds(3);

            context.PreFightAnimationObject.SetActive(false);


            BattleStatesManager.SetCurrentState<EnemyTurnBattleState>();

            // TO REWORK!!!
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
        [SerializeField] public List<Transform> EnemiesSlots;
        [SerializeField] public List<ActorFactoryRequest> EnemiesToSpawn;

        [Header("Heroes setup")]
        [SerializeField] public List<Transform> HeroesSlots;
        [SerializeField] public List<ActorFactoryRequest> HeroesToSpawn;


        [Header("TO REMOVE LATER")]
        [SerializeField] public DynamicSelectablesEventHandler Skills;

        #region BattleStatesData
        public List<Enemy> EnemiesInBattle = new();
        public List<Hero> HeroesInBattle = new();

        public EnemiesStatesEventHandler EnemiesEventHandler = new();

        #endregion

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
