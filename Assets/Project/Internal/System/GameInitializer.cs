using System.Collections;
using System.Collections.Generic;
using Project.Internal.ActorSystem;
using Project.Internal.Interactions;
using Project.Internal.SkillsSystem;
using Project.Internal.Utilities;
using UnityEngine;

namespace Project.Internal.System
{
    public class GameInitInteraction : InteractionBase, IOnGameInitStart
    {
        public IEnumerator OnGameInit(GameInitializer context)
        {
            //context.GameLoadingScreen.SetActive(true);

            yield return context.InputManager.Init();

            yield return context.ActorsVisualsRegistry.Init();

            yield return context.ToolTipManager.Init();
            yield return context.SkillsVisualsRegistry.Init();

            //context.GameLoadingScreen.SetActive(false);
        }
    }


    public class GameInitEndInteraction : InteractionBase, IOnGameInitEnd
    {
        public IEnumerator OnGameInitEnd(GameInitializer context)
        {
            ScenesTransitionManager.ST_SingleMode_WithLoadingScreenAsync(context.AfterLoadSceneName, context.SceneTransitionTemplateID);
            yield return null;
        }
    }

    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] public InputManager InputManager;
        [SerializeField] public ActorsVisualsRegistry ActorsVisualsRegistry;
        [SerializeField] public SkillsVisualsRegistry SkillsVisualsRegistry;
        [SerializeField] public ToolTipManager ToolTipManager;


        [Header("Game initializing setups")]
        [SerializeField] public GameObject GameLoadingScreen;

        [Header("Game starting setups")]
        [SerializeField] public string AfterLoadSceneName;
        [SerializeField] public string SceneTransitionTemplateID;

        IEnumerator Start()
        {
            InteractionManager interactManager = new InteractionManager();
            interactManager.Init();

            Debug.Log("Game init START");
            foreach (var interaction in interactManager.FindAllOf<IOnGameInitStart>())
            {
                yield return interaction.OnGameInit(this);
            }


            Debug.Log("Game init END");
            foreach (var interaction in interactManager.FindAllOf<IOnGameInitEnd>())
            {
                yield return interaction.OnGameInitEnd(this);
            }

            yield return null;
        }
    }
}
