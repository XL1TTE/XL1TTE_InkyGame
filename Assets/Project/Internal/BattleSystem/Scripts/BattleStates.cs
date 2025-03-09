using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using Project.Internal.ActorSystem;
using Project.Internal.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Internal.BattleSystem
{
    public abstract class BaseBattleState
    {
        public abstract void ApplyStateLogic();

        /// <summary>
        /// Attaches EventTriggerComponent to object if any had not been found on the object. 
        /// </summary>
        protected virtual EventTrigger AttachEventTriggerComponent(GameObject item)
        {
            var trigger = item.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = item.AddComponent<EventTrigger>();
            }
            return trigger;
        }
    }


    public class PlayerTurnBattleState : BaseBattleState
    {
        protected List<Enemy> EnemiesInBattle;
        protected List<Hero> HeroesInBattle;

        public PlayerTurnBattleState(List<Enemy> EnemiesInBattle, List<Hero> HeroesInBattle)
        {
            this.EnemiesInBattle = EnemiesInBattle;
            this.HeroesInBattle = HeroesInBattle;
        }

        public override void ApplyStateLogic()
        {
            ApplyTooltipsToEnemies();
        }

        protected void ApplyTooltipsToEnemies()
        {
            foreach (var item in EnemiesInBattle)
            {
                var trigger = base.AttachEventTriggerComponent(item.gameObject);
                EventTrigger.Entry PointerEnterEntry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerEnter
                };
                PointerEnterEntry.callback.AddListener(OnEnemyPointerEnter);
                trigger.triggers.Add(PointerEnterEntry);

                EventTrigger.Entry PointerExitEntry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerExit
                };
                PointerExitEntry.callback.AddListener(OnEnemyPointerExit);
                trigger.triggers.Add(PointerExitEntry);
            }
        }

        private void OnEnemyPointerExit(BaseEventData eventData)
        {
            ToolTipManager.HideTooltip();
        }

        private void OnEnemyPointerEnter(BaseEventData eventData)
        {
            PointerEventData pointerEventData = eventData as PointerEventData;

            var enemy = pointerEventData.pointerEnter.GetComponent<Enemy>();
            if (enemy == null)
            {
                return;
            }
            var enemy_data = enemy.GetActorData<EnemyData>();
            ToolTipManager.ShowTooltip(enemy_data.ActorName, $"{enemy_data.GetAllStatsInString()}");
        }
    }
}
