using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Internal.ActorSystem;
using Project.Internal.BattleSystem.States;
using Project.Internal.System;
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

        protected virtual void OnPointerExit(BaseEventData eventData)
        {

        }

        protected virtual void OnPointerEnter(BaseEventData eventData)
        {

        }

        protected virtual void OnDeselect(BaseEventData eventData)
        {
            Selectable selectable = eventData.selectedObject.GetComponent<Selectable>();
        }

        protected virtual void OnSelect(BaseEventData eventData)
        {
            _lastSelected = eventData.selectedObject.GetComponent<Selectable>();

        }



        protected virtual void OnNavigate(InputAction.CallbackContext context)
        {
            if (EventSystem.current.currentSelectedGameObject == null && _lastSelected != null)
            {
                EventSystem.current.SetSelectedGameObject(_lastSelected.gameObject);
            }
        }
    }


    public class EnemiesStatesEventHandler : SelectablesStatesEventHandler<Enemy>
    {

        protected override void OnPointerEnter(BaseEventData eventData)
        {
            PointerEventData pointerEventData = eventData as PointerEventData;

            var enemy = pointerEventData.pointerEnter.GetComponent<Enemy>();
            if (enemy != null)
                BattleStatesManager.CurrentState.OnEnemyPointerEnter(enemy);
        }

        protected override void OnPointerExit(BaseEventData eventData)
        {

            PointerEventData pointerEventData = eventData as PointerEventData;

            var enemy = pointerEventData.pointerEnter.GetComponent<Enemy>();
            if (enemy != null)
                BattleStatesManager.CurrentState.OnEnemyPointerExit(enemy);
        }
    }


}
