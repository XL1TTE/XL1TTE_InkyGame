using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Internal.SkillsSystem
{
    public class SkillsVisualsRegistry : MonoBehaviour
    {
        public static SkillsVisualsRegistry instance;


        public List<SkillVisual> SkillVisuals = new();


        public IEnumerator Init()
        {
            if (instance == null)
            {
                instance = this;
            }
            yield return null;
        }


        public SkillVisual GetSkillVisualByID(string ID)
        {
            return SkillVisuals.Find(s => s.ID == ID);
        }
    }
}
