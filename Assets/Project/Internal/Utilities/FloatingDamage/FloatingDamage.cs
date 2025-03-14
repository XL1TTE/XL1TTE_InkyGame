using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Project.Internal.Utilities
{
    public class FloatingDamage : MonoBehaviour
    {
        public float moveSpeed = 1f;
        public float fadeDuration = 1f;

        [SerializeField] public Vector3 DisappearShift;

        [SerializeField] public Vector3 StartPositionShift;
        [SerializeField] public TextMeshPro DamageText;
    }
}
