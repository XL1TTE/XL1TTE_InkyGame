using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Template_01", menuName = "LoadingScreen/LoadingScreenData")]
public class LoadingScreenData : ScriptableObject
{
    public List<Sprite> Pictures;

    public List<string> TipMessages;
}

