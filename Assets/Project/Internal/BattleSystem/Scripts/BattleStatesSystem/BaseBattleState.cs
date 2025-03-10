using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Internal.ActorSystem;
using Project.Internal.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Internal.BattleSystem.States
{
    public abstract class BaseBattleState
    {
        protected Tween _scaleUpTween;
        protected Tween _scaleDownTween;

        protected Vector3 _defaultSkillScale;

        public virtual void OnSkillPointerEnter(GameObject skill)
        {
            _defaultSkillScale = skill.transform.localScale;
            Vector3 newScale = skill.transform.localScale * 1.1f;
            _scaleUpTween = skill.transform.DOScale(newScale, 0.25f);
        }

        public virtual void OnSkillPointerExit(GameObject skill)
        {

            _scaleDownTween = skill.transform.DOScale(_defaultSkillScale, 0.25f);
        }

        public virtual void OnEnemyPointerExit(Enemy enemy)
        {
            ToolTipManager.HideTooltip();
        }

        public virtual void OnEnemyPointerEnter(Enemy enemy)
        {
            if (enemy == null)
            {
                return;
            }
            var enemy_data = enemy.GetActorData<EnemyData>();
            ToolTipManager.ShowTooltip(enemy_data.ActorName, $"{enemy_data.GetAllStatsInString()}");
        }
    }

    public class PlayerTurnBattleState : BaseBattleState
    {

    }

    public class EnemyTurnBattleState : BaseBattleState
    {
        public override void OnEnemyPointerEnter(Enemy enemy)
        {
            Debug.Log("Enemy Turn!!!");
        }
    }


}
