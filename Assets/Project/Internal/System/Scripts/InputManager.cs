using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Project.Internal.System
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] public InputActionReference UI_NavigationScheme;

        public static InputManager instance;


        public IEnumerator Init()
        {
            Debug.Log("Initializing InputManager...");
            if (instance == null)
            {
                instance = this;
            }
            yield return null;
        }
    }

}
