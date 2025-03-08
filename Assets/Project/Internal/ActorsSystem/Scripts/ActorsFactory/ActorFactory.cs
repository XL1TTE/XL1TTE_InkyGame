using System.Collections;
using System.Collections.Generic;
using Project.Internal.Utilities;
using UnityEngine;

namespace Project.Internal.ActorSystem
{
    public static class ActorFactory
    {

        public static bool TryBuildHero(string actorID, bool isDisableByDefault, out Hero hero)
        {
            var HeroPrefab = ActorsVisualsRegistry.instance.GetHeroPrefabByID(actorID);

            if (HeroPrefab != null)
            {
                var HeroData = ActorsDataRegistry.GetHeroDataByID(actorID);


                var buildedHero = Object.Instantiate(HeroPrefab);
                buildedHero.AttachActorData(HeroData);

                buildedHero.gameObject.SetActive(!isDisableByDefault);

                hero = buildedHero;
                return true;
            }
            Debug.LogWarning($"You tried to spawn actor with id:{actorID}, but visual for that id was not registred.");
            hero = null;
            return false;
        }

    }
}
