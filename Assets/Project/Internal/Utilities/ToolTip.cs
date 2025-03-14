using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Project.Internal.Utilities
{
    public class ToolTip : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI Header;
        [SerializeField] public TextMeshProUGUI Message;

        public void Show()
        {
            UpdateTooltipPosition();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UpdateTooltipPosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            transform.position = new Vector3(mousePosition.x + 10, mousePosition.y - 10, mousePosition.z);
        }

        private void Update()
        {
            UpdateTooltipPosition();
        }
    }
}
