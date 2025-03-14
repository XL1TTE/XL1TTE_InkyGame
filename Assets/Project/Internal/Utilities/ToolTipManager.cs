using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Internal.Utilities
{

    public class ToolTipManager : MonoBehaviour
    {
        public ToolTip TooltipInstance;
        private static ToolTip tooltipInstance;

        public IEnumerator Init()
        {
            if (tooltipInstance == null && TooltipInstance != null)
            {
                tooltipInstance = TooltipInstance;

            }
            tooltipInstance.Hide();
            yield return null;
        }

        public static void ShowTooltip(string header, string message)
        {
            if (tooltipInstance != null)
            {
                tooltipInstance.Header.text = header;
                tooltipInstance.Message.text = message;

                tooltipInstance.Show();
            }
        }

        public static void HideTooltip()
        {
            if (tooltipInstance != null)
            {
                tooltipInstance.Hide();
            }
        }

    }
}
