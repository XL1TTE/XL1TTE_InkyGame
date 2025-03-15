using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Project.Internal.ActorSystem;
using Project.Internal.Interfaces;
using Project.Internal.SkillsSystem;
using Project.Internal.UI;
using Project.Internal.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Internal.BattleSystem.States
{
    public abstract class BaseBattleState
    {
        protected Tween _ActorScaleUpTween;
        protected Tween _ActorScaleDownTween;

        protected Vector3 _defaultEnemyScale;



        #region Skills

        public virtual void OnSkillPointerEnter(SkillSlot skill, BattleManager context)
        {
            EventSystem.current.SetSelectedGameObject(skill.gameObject);
        }

        public virtual void OnSkillPointerExit(SkillSlot skill, BattleManager context)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        public virtual void OnSkilPointerClick(SkillSlot skill, BattleManager context)
        {
            var skill_state = BattleStatesManager.SetCurrentState<SkillUsingBattleState>();

            skill_state.Setup(BattleStatesManager.LastSelectedSkillUser, skill);
        }

        public virtual void OnSkillSelect(SkillSlot skill, BattleManager context)
        {
            skill.DOSelectAnimation();
        }
        public virtual void OnSkillDeselect(SkillSlot skill, BattleManager context)
        {
            skill.DODeselectAnimation();
        }

        public virtual void OnSkillSubmit(SkillSlot skill, BattleManager context)
        {
            OnSkilPointerClick(skill, context);
        }

        #endregion

        #region Enimies

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

        public virtual IEnumerator OnEnemyLeftMouseButtonClick(Enemy enemy, BattleManager context)
        {
            yield return null;
        }

        public virtual IEnumerator OnEmemyRightMouseButtonClick(Enemy enemy, BattleManager context)
        {
            yield return null;
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
            Vector3 newScale = enemy.gameObject.transform.localScale * 1.1f;
            _ActorScaleUpTween = enemy.gameObject.transform.DOScale(newScale, 0.25f);
        }
        public virtual void OnEnemyDeselect(Enemy enemy, BattleManager context)
        {
            _ActorScaleDownTween = enemy.gameObject.transform.DOScale(1, 0.25f);
        }
        #endregion

        #region Heroes

        public virtual void OnHeroSelect(Hero hero, BattleManager context)
        {
            BattleStatesManager.LastSelectedSkillUser = hero;
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

        #endregion
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

        public override void OnSkillSelect(SkillSlot skill, BattleManager context)
        {

            base.OnSkillSelect(skill, context);

        }
        public override void OnSkillDeselect(SkillSlot skill, BattleManager context)
        {
            base.OnSkillDeselect(skill, context);
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
            ToolTipManager.ShowTooltip(enemy_data.ActorName, $"{enemy_data.Stats.DamageStats.PhysicalDamage} урона!");
        }


        public override void OnEnemyPointerExit(Enemy enemy, BattleManager context)
        {
            base.OnEnemyPointerExit(enemy, context);
        }
    }


    public class SkillUsingBattleState : BaseBattleState
    {

        protected bool IsAnySkillExecuting = false;
        protected BaseSkill UsingSkill;
        protected SkillSlot SkillSlot;

        protected ISkillsUser SkillOwner;
        protected List<IDamagable> Targets = new();

        public void Setup(ISkillsUser SkillOwner, SkillSlot skill_slot)
        {
            SkillSlot = skill_slot;
            SkillSlot.LockAnimationState(true);

            UsingSkill = skill_slot.AttachedSkill;

            this.SkillOwner = SkillOwner;

            if (UsingSkill != null)
                ToolTipManager.ShowTooltip($"Targets:", $"{Targets.Count}/{UsingSkill.SkillInfo.MaxTargets}");
        }

        public override void OnSkillSubmit(SkillSlot skill, BattleManager context)
        {
            if (skill != SkillSlot)
            {
                SkillSlot.LockAnimationState(false);
                SkillSlot.DODeselectAnimation();
            }
            base.OnSkillSubmit(skill, context);
        }

        public override void OnSkilPointerClick(SkillSlot skill, BattleManager context)
        {

            if (IsAnySkillExecuting)
            {
                return;
            }

            if (skill != SkillSlot)
            {
                SkillSlot.LockAnimationState(false);
                SkillSlot.DODeselectAnimation();
            }
            base.OnSkilPointerClick(skill, context);
        }

        public override IEnumerator OnEnemyLeftMouseButtonClick(Enemy enemy, BattleManager context)
        {

            if (UsingSkill == null || IsAnySkillExecuting)
            {
                yield break;
            }


            if (Targets.Count < UsingSkill.SkillInfo.MaxTargets)
            {
                Targets.Add(enemy);
            }

            ToolTipManager.ShowTooltip($"Targets:", $"{Targets.Count}/{UsingSkill.SkillInfo.MaxTargets}");

            if (Targets.Count == UsingSkill.SkillInfo.MaxTargets)
            {
                IsAnySkillExecuting = true;

                ToolTipManager.HideTooltip();
                yield return context.StartCoroutine(UsingSkill.Execute(Targets, SkillOwner));

                IsAnySkillExecuting = false;

                Targets.Clear();

                ComebackToPlayerTurnState();
            }


        }
        private void ComebackToPlayerTurnState()
        {

            SkillSlot.LockAnimationState(false);
            SkillSlot.DODeselectAnimation();

            BattleStatesManager.SetCurrentState<PlayerTurnBattleState>();
        }

        public override IEnumerator OnEmemyRightMouseButtonClick(Enemy enemy, BattleManager context)
        {
            if (Targets.Count == 0)
            {
                ComebackToPlayerTurnState();
                yield return null;
            }

            if (Targets.Count > 0 && Targets.Contains(enemy))
            {
                Targets.Remove(enemy);
            }
            ToolTipManager.ShowTooltip($"Targets:", $"{Targets.Count}/{UsingSkill.SkillInfo.MaxTargets}");
        }


        public override void OnHeroSelect(Hero hero, BattleManager context)
        {
        }
        public override void OnHeroDeselect(Hero hero, BattleManager context)
        {
        }

        public override void OnEnemySelect(Enemy enemy, BattleManager context)
        {

        }

        public override void OnEnemyDeselect(Enemy enemy, BattleManager context)
        {

        }

        public override void OnEnemyPointerEnter(Enemy enemy, BattleManager context)
        {

        }

        public override void OnEnemyPointerExit(Enemy enemy, BattleManager context)
        {

        }

    }

}
