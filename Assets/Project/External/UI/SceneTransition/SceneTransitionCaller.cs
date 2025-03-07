using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SceneTransitionCaller : MonoBehaviour
{
    public string CallableTransitionID;

    public void CallSceneTransition(string SceneName)
    {
        ScenesTransitionManager.ST_SingleMode_WithLoadingScreenAsync(SceneName, CallableTransitionID);
    }

}
