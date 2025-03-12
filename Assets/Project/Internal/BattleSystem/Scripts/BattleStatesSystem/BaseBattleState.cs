using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Internal.ActorSystem;
using Project.Internal.SkillsSystem;
using Project.Internal.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Internal.BattleSystem.States
{
    public abstract class BaseBattleState
    {
        protected Tween _SkillscaleUpTween;
        protected Tween _SkillscaleDownTween;

        protected Tween _ActorScaleUpTween;
        protected Tween _ActorScaleDownTween;

        protected Vector3 _defaultSkillScale;
        protected Vector3 _defaultEnemyScale;

        public virtual void OnSkillPointerEnter(GameObject skill, BattleManager context)
        {
            _SkillscaleDownTween.Kill();
            _defaultSkillScale = skill.transform.localScale;
            Vector3 newScale = skill.transform.localScale * 1.1f;
            _SkillscaleUpTween = skill.transform.DOScale(newScale, 0.25f);
        }

        public virtual void OnSkillPointerExit(GameObject skill, BattleManager context)
        {
            _SkillscaleUpTween.Kill();
            _SkillscaleDownTween = skill.transform.DOScale(_defaultSkillScale, 0.25f);
        }

        public virtual void OnEnemyPointerExit(Enemy enemy, BattleManager context)
        {
            ToolTipManager.HideTooltip();
        }

        public virtual void OnEnemyPointerEnter(Enemy enemy, BattleManager context)
        {
            if (enemy == null)
            {
                return;
            }
            var enemy_data = enemy.GetActorData<EnemyData>();
            ToolTipManager.ShowTooltip(enemy_data.ActorName, $"{enemy_data.GetAllStatsInString()}");
        }

        public virtual void OnEnemySelect(Enemy enemy, BattleManager context)
        {
            var enemy_data = enemy.GetActorData<EnemyData>();
            if (enemy_data != null)
            {
                context.UI.ActorAvatarFrame.sprite = enemy.Avatar;
                context.UI.ActorNameTextMesh.text = enemy_data.ActorName;
                context.UI.ActorStatsTextMesh.text = enemy_data.GetAllStatsInString();
            }

            _SkillscaleDownTween.Kill();
            _defaultEnemyScale = enemy.gameObject.transform.localScale;
            Vector3 newScale = enemy.gameObject.transform.localScale * 1.1f;
            _SkillscaleUpTween = enemy.gameObject.transform.DOScale(newScale, 0.25f);
        }
        public virtual void OnEnemyDeselect(Enemy enemy, BattleManager context)
        {
            _ActorScaleUpTween.Kill();
            _ActorScaleDownTween = enemy.gameObject.transform.DOScale(_defaultEnemyScale, 0.25f);
        }

        public virtual void OnHeroSelect(Hero hero, BattleManager context)
        {
            var hero_data = hero.GetActorData<HeroData>();
            if (hero != null)
            {
                context.UI.ActorAvatarFrame.sprite = hero.Avatar;
                context.UI.ActorNameTextMesh.text = hero_data.ActorName;
                context.UI.ActorStatsTextMesh.text = hero_data.GetAllStatsInString();
            }
        }

        public virtual void OnHeroDeselect(Hero hero, BattleManager context)
        {

        }
    }

    public class PlayerTurnBattleState : BaseBattleState
    {
        public override void OnEnemySelect(Enemy enemy, BattleManager context)
        {
            base.OnEnemySelect(enemy, context);
        }

        public override void OnEnemyDeselect(Enemy enemy, BattleManager context)
        {
            base.OnEnemyDeselect(enemy, context);
        }

        public override void OnHeroSelect(Hero hero, BattleManager context)
        {
            base.OnHeroSelect(hero, context);


            //CLEARING OUTDATED SKILLS IN UI
            foreach (var skill_slot in context.UI.ActorSkills_Frames)
            {
                skill_slot.SkillIconImage.gameObject.SetActive(false);
                skill_slot.AttachedSkill = null;
            }

            // FILLING WITH NEWS
            int skill_index = 0;

            var skills_data = hero.GetActorData<HeroData>().Skills;
            foreach (var skill in skills_data)
            {
                if (skill != null)
                {
                    var skill_visual_id = skill.SkillInfo.SkillVisualID;
                    var skill_visual = SkillsVisualsRegistry.instance.GetSkillVisualByID(skill_visual_id);
                    if (context.UI.ActorSkills_Frames.Count >= skill_index + 1)
                    {
                        var slot = context.UI.ActorSkills_Frames[skill_index++];

                        slot.SkillIconImage.sprite = skill_visual.SkillIcon;

                        slot.AttachedSkill = skill;

                        slot.SkillIconImage.gameObject.SetActive(true);
                    }
                }

            }
        }
        public override void OnHeroDeselect(Hero hero, BattleManager context)
        {
            base.OnHeroDeselect(hero, context);
        }
    }

    public class EnemyTurnBattleState : BaseBattleState
    {
        public override void OnEnemyPointerEnter(Enemy enemy, BattleManager context)
        {
            if (enemy == null)
            {
                return;
            }
            var enemy_data = enemy.GetActorData<EnemyData>();
            ToolTipManager.ShowTooltip(enemy_data.ActorName, $"{enemy_data.Stats.PhysicalDamage} урона!");
        }


        public override void OnEnemyPointerExit(Enemy enemy, BattleManager context)
        {
            base.OnEnemyPointerExit(enemy, context);
        }
    }
}
