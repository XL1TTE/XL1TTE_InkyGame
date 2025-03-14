using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Internal.Utilities
{
    public class ColorsUtility
    {
        public static Color GetColorFromHex(string hex)
        {
            // Убираем символ '#' в начале, если он есть
            if (hex.StartsWith("#"))
            {
                hex = hex.Substring(1);
            }

            // Преобразуем hex в Color
            Color color;
            if (UnityEngine.ColorUtility.TryParseHtmlString("#" + hex, out color))
            {
                return color;
            }
            else
            {
                Debug.LogError("Invalid hex color: " + hex);
                return color;
            }
        }
    }
}
