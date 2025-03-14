using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Internal.ActorSystem;
using Project.Internal.BattleSystem;
using Project.Internal.BattleSystem.States;
using Project.Internal.SkillsSystem;
using Project.Internal.System;
using Project.Internal.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Project.Internal
{
    public abstract class SelectablesStatesEventHandler<SubjectType> where SubjectType : MonoBehaviour
    {

        protected List<Selectable> _selectables = new();

        protected Selectable _firstSelected;
        protected Selectable _lastSelected;

        protected InputActionReference NavigationScheme;


        /// <summary>
        /// Initialize default selectables state
        /// </summary>
        /// 
        public void Apply()
        {
            if (InputManager.instance != null)
            {
                NavigationScheme = InputManager.instance.UI_NavigationScheme;
            }
            else
            {
                Debug.LogWarning("InputManager was not initialized...");
            }

            foreach (var selectable in _selectables)
            {
                AddListeners(selectable);
            }

            _firstSelected = _selectables[0];
        }

        public virtual IEnumerator SelectFirstAfterDelay()
        {
            yield return null;
            if (_firstSelected != null)
                EventSystem.current.SetSelectedGameObject(_firstSelected.gameObject);
        }

        public virtual void AddSubject(SubjectType subject)
        {
            var sel = subject.gameObject.GetComponent<Selectable>();
            if (sel == null)
            {
                sel = subject.gameObject.AddComponent<Selectable>();
            }
            _selectables.Add(sel);
        }

        protected virtual void AddListeners(Selectable selectable)
        {
            EventTrigger trigger = selectable.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = selectable.gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry SelectEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.Select
            };
            SelectEntry.callback.AddListener(OnSelect);
            trigger.triggers.Add(SelectEntry);

            EventTrigger.Entry DeselectEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.Deselect
            };
            DeselectEntry.callback.AddListener(OnDeselect);
            trigger.triggers.Add(DeselectEntry);

            EventTrigger.Entry PointerEnterEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            PointerEnterEntry.callback.AddListener(OnPointerEnter);
            trigger.triggers.Add(PointerEnterEntry);

            EventTrigger.Entry PointerExitEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            PointerExitEntry.callback.AddListener(OnPointerExit);
            trigger.triggers.Add(PointerExitEntry);

            EventTrigger.Entry OnPointerClickEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };
            OnPointerClickEntry.callback.AddListener(OnPointerClick);
            trigger.triggers.Add(OnPointerClickEntry);

            EventTrigger.Entry OnSubmitEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.Submit
            };
            OnSubmitEntry.callback.AddListener(OnSubmit);
            trigger.triggers.Add(OnSubmitEntry);

        }


        protected virtual void RemoveListeners(Selectable selectable)
        {
            var trigger = selectable.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                return;
            }
            trigger.triggers.Clear();
        }


        protected virtual void OnPointerClick(BaseEventData eventData)
        {

        }

        protected virtual void OnPointerExit(BaseEventData eventData)
        {

        }

        protected virtual void OnPointerEnter(BaseEventData eventData)
        {

        }

        protected virtual void OnDeselect(BaseEventData eventData)
        {
        }

        protected virtual void OnSelect(BaseEventData eventData)
        {
        }

        protected virtual void OnSubmit(BaseEventData eventData)
        {
        }

    }


    public class EnemiesStatesEventHandler : SelectablesStatesEventHandler<Enemy>
    {
        protected BattleManager Context;
        public EnemiesStatesEventHandler(BattleManager context)
        {
            Context = context;
        }

        protected override void OnPointerEnter(BaseEventData eventData)
        {
            PointerEventData pointerEventData = eventData as PointerEventData;

            var enemy = pointerEventData.pointerEnter.GetComponent<Enemy>();
            if (enemy != null)
                BattleStatesManager.CurrentState.OnEnemyPointerEnter(enemy, Context);
        }

        protected override void OnPointerExit(BaseEventData eventData)
        {

            PointerEventData pointerEventData = eventData as PointerEventData;

            var enemy = pointerEventData.pointerEnter.GetComponent<Enemy>();
            if (enemy != null)
                BattleStatesManager.CurrentState.OnEnemyPointerExit(enemy, Context);
        }

        protected override void OnPointerClick(BaseEventData eventData)
        {
            PointerEventData pointerEventData = eventData as PointerEventData;

            var enemy = pointerEventData.pointerEnter.GetComponent<Enemy>();

            if (pointerEventData != null && enemy != null)
            {
                switch (pointerEventData.button)
                {
                    case PointerEventData.InputButton.Left:
                        BattleStatesManager.CurrentState.OnEnemyLeftMouseButtonClick(enemy, Context);
                        break;
                    case PointerEventData.InputButton.Right:
                        BattleStatesManager.CurrentState.OnEmemyRightMouseButtonClick(enemy, Context);
                        break;
                }

            }
        }

        protected override void OnSelect(BaseEventData eventData)
        {
            var enemy = eventData.selectedObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                BattleStatesManager.CurrentState.OnEnemySelect(enemy, Context);
            }
        }

        protected override void OnDeselect(BaseEventData eventData)
        {
            var enemy = eventData.selectedObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                BattleStatesManager.CurrentState.OnEnemyDeselect(enemy, Context);
            }
        }
    }

    public class HeroesStatesEventHandler : SelectablesStatesEventHandler<Hero>
    {
        protected BattleManager Context;
        public HeroesStatesEventHandler(BattleManager context)
        {
            Context = context;
        }


        protected override void OnSelect(BaseEventData eventData)
        {
            var hero = eventData.selectedObject.GetComponent<Hero>();
            if (hero != null)
                BattleStatesManager.CurrentState.OnHeroSelect(hero, Context);
        }

        protected override void OnDeselect(BaseEventData eventData)
        {
            var hero = eventData.selectedObject.GetComponent<Hero>();
            if (hero != null)
                BattleStatesManager.CurrentState.OnHeroDeselect(hero, Context);
        }
    }

    public class HeroSkillsStatesEventHandler : SelectablesStatesEventHandler<SkillSlot>
    {
        public BattleManager Context;
        public HeroSkillsStatesEventHandler(BattleManager context)
        {
            Context = context;
        }

        protected override void OnPointerClick(BaseEventData eventData)
        {
            var skill = eventData.selectedObject.GetComponent<SkillSlot>();

            if (skill != null)
            {
                BattleStatesManager.CurrentState.OnSkilPointerClick(skill, Context);
            }
        }

        protected override void OnPointerEnter(BaseEventData eventData)
        {
            PointerEventData pointerEventData = eventData as PointerEventData;

            var skill_slot = pointerEventData.pointerEnter.GetComponentInParent<SkillSlot>();

            if (skill_slot != null)
            {
                BattleStatesManager.CurrentState.OnSkillPointerEnter(skill_slot, Context);
            }
        }

        protected override void OnPointerExit(BaseEventData eventData)
        {
            PointerEventData pointerEventData = eventData as PointerEventData;

            var skill_slot = pointerEventData.pointerEnter.GetComponentInParent<SkillSlot>();

            if (skill_slot != null)
            {
                BattleStatesManager.CurrentState.OnSkillPointerExit(skill_slot, Context);
            }
        }
        protected override void OnSelect(BaseEventData eventData)
        {
            var skill = eventData.selectedObject.GetComponent<SkillSlot>();

            if (skill != null /*&& skill.AttachedSkill != null*/)
            {
                BattleStatesManager.CurrentState.OnSkillSelect(skill, Context);
            }
        }

        protected override void OnDeselect(BaseEventData eventData)
        {
            var skill = eventData.selectedObject.GetComponent<SkillSlot>();

            if (skill != null)
            {
                BattleStatesManager.CurrentState.OnSkillDeselect(skill, Context);
            }
        }


        protected override void OnSubmit(BaseEventData eventData)
        {
            var skill = eventData.selectedObject.GetComponent<SkillSlot>();

            if (skill != null)
            {
                BattleStatesManager.CurrentState.OnSkillSubmit(skill, Context);
            }
        }

    }


}
