using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Internal.UI.SceneTransition
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void OnEnable()
        {
            DontDestroyOnLoad(this.gameObject);
        }

    }
}
