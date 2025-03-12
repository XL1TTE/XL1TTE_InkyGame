using System;
using System.Collections;
using System.Collections.Generic;
using Project.Internal.ActorSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Internal.UI
{
    [Serializable]
    public class BattleUI_Context
    {
        public Image ActorAvatarFrame;
        public TextMeshProUGUI ActorNameTextMesh;
        public TextMeshProUGUI ActorStatsTextMesh;

        public List<Image> ActorTurnsQueue_Frames;
        public List<SkillSlot> ActorSkills_Frames;
    }
}
