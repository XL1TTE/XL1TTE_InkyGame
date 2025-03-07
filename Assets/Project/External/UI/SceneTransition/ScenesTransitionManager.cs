using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesTransitionManager : MonoBehaviour
{
    private static ScenesTransitionManager _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static ScenesTransitionManager Instance()
    {
        return _instance;
    }

    [SerializeField]
    private List<RegisteredTemplate> TransitionTemplates;

    public bool TryRegisterNewTemplate(string ID, ABS_SceneTransition template)
    {
        foreach (var item in _instance.TransitionTemplates)
        {
            if (item.TemplateID == ID)
            {
                return false;
            }
        }

        var NewTemplate = new RegisteredTemplate
        {
            TemplateID = ID,
            TransitionTemplate = template
        };

        _instance.TransitionTemplates.Add(NewTemplate);

        return true;

    }

    private bool TryGetTransition(string TemplateID, out ABS_SceneTransition result)
    {
        foreach (var item in _instance.TransitionTemplates)
        {
            if (item.TemplateID == TemplateID)
            {
                result = item.TransitionTemplate;
                return true;
            }
        }
        result = null;
        return false;
    }

    public static void ST_SingleMode_WithLoadingScreenAsync(string SceneName, string TemplateID)
    {
        var _instance = Instance();

        try
        {
            if (_instance.TryGetTransition(TemplateID, out var result))
            {
                AsyncOperation LoadingState = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
                LoadingState.allowSceneActivation = false;

                result.sceneLoadingState = LoadingState;
                result.StartTransition();
            }
            else
            {
                Debug.LogError("Provided somewhere TemplateID for scene transition was not registred.");
                return;
            }

        }
        catch
        {
            Debug.LogWarning("Transition Manager was not initialized.");
        }
    }

}
