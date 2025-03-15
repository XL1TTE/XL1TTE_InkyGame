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

                    enemy.OnDamageTaken += FloatingDamageManager.instance.OnEnemyDamageTaken;
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

                    context.HeroesEventHandler.AddSubject(hero);

                    context.HeroesInBattle.Add(hero);
                    yield return null;
                }
            }

            // SETUP HEROES SKILLS SLOTS
            foreach (var skill_slot in context.UI.ActorSkills_Frames)
            {
                context.SkillsEventHandler.AddSubject(skill_slot);
            }


            context.HeroesEventHandler.Apply();
            context.SkillsEventHandler.Apply();

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
                yield return null;
            }

            // Spawning Enemies
            slot_i = 0;
            foreach (var enemy in context.EnemiesInBattle)
            {
                SpawnerUtility.SpawnItemIn(enemy.gameObject, context.EnemiesSlots[slot_i], true);
                slot_i++;
                yield return null;
            }

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


        #region BattleStatesData
        [HideInInspector] public List<Enemy> EnemiesInBattle = new();
        [HideInInspector] public List<Hero> HeroesInBattle = new();

        public EnemiesStatesEventHandler EnemiesEventHandler;
        public HeroesStatesEventHandler HeroesEventHandler;
        public HeroSkillsStatesEventHandler SkillsEventHandler;
        public BattleUI_Context UI;

        #endregion

        IEnumerator Start()
        {

            EnemiesEventHandler = new EnemiesStatesEventHandler(this);

            HeroesEventHandler = new HeroesStatesEventHandler(this);

            SkillsEventHandler = new HeroSkillsStatesEventHandler(this);

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
