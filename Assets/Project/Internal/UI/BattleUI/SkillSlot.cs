using System.Collections;
using System.Collections.Generic;
using Project.Internal.SkillsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Internal.UI
{
    public class SkillSlot : MonoBehaviour
    {
        [SerializeField] public Image SkillIconImage;

        [HideInInspector] public BaseSkill AttachedSkill;
    }
}
