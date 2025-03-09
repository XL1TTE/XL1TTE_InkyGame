using System.Collections;
using System.Collections.Generic;
using Project.Internal.Utilities;
using UnityEngine;

namespace Project.Internal.ActorSystem
{
    public static class ActorFactory
    {

        public static bool TryBuildHero(string actorDataID, string actorPrefabID, bool isDisableByDefault, out Hero hero)
        {
            if (ActorsVisualsRegistry.instance == null)
            {
                Debug.LogWarning("ActorVisualsRegistry was not initialized, so ActorBuilder won't be able to work...");
                hero = null;
                return false;
            }
            var HeroPrefab = ActorsVisualsRegistry.instance.GetHeroPrefabByID(actorPrefabID);

            if (HeroPrefab != null)
            {
                var HeroData = ActorsDataRegistry.GetHeroDataByID(actorDataID).Clone<HeroData>();


                var buildedHero = Object.Instantiate(HeroPrefab);
                buildedHero.AttachActorData(HeroData);

                buildedHero.gameObject.SetActive(!isDisableByDefault);

                hero = buildedHero;
                return true;
            }
            Debug.LogWarning($"You tried to spawn actor with data id:{actorDataID}, but visual with id: {actorPrefabID} was not registred.");
            hero = null;
            return false;
        }

        public static bool TryBuildEnemy(string actorDataID, string actorPrefabID, bool isDisableByDefault, out Enemy enemy)
        {
            if (ActorsVisualsRegistry.instance == null)
            {
                Debug.LogWarning("ActorVisualsRegistry was not initialized, so ActorBuilder won't be able to work...");
                enemy = null;
                return false;
            }
            var EnemyPrefab = ActorsVisualsRegistry.instance.GetEnemyPrefabByID(actorPrefabID);

            if (EnemyPrefab != null)
            {
                var EnemyData = ActorsDataRegistry.GetEnemyDataByID(actorDataID).Clone<EnemyData>();


                var buildedEnemy = Object.Instantiate(EnemyPrefab);
                buildedEnemy.AttachActorData(EnemyData);

                buildedEnemy.gameObject.SetActive(!isDisableByDefault);

                enemy = buildedEnemy;
                return true;
            }
            Debug.LogWarning($"You tried to spawn actor with data id:{actorDataID}, but visual with id: {actorPrefabID} was not registred.");
            enemy = null;
            return false;
        }

    }
}
