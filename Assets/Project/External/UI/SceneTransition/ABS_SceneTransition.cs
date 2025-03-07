using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABS_SceneTransition : MonoBehaviour
{
    [HideInInspector]
    public AsyncOperation sceneLoadingState;


    /// <summary>
    /// In that method you call all of your transition logic which should be executed when scene transition happened.
    /// Don't forget to set allowSceneActivation of sceneLoadingState in true! 
    /// </summary>
    /// <param name="SceneName"></param>
    public abstract void StartTransition();
}
