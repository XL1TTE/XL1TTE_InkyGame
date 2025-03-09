using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PlasticGui.EventTracking;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Project.Internal.System;

namespace Project.Internal.UI
{
    public class DynamicSelectablesEventHandler : MonoBehaviour
    {


        [Header("StandartBehaviour")]
        [SerializeField] protected float _scaleMultiplier;
        [SerializeField] protected float _scaleTransitionTime;

        [Header("Static selectables setup")]
        [SerializeField] public List<Selectable> _selectables = new();

        [HideInInspector] protected Dictionary<Selectable, Vector3> _defaultScales = new();


        [HideInInspector] protected Selectable _firstSelected;
        [HideInInspector] protected Selectable _lastSelected;

        /// Animations tweens...
        [HideInInspector] protected Tween _scaleUpTween;
        [HideInInspector] protected Tween _scaleDownTween;


        [HideInInspector] protected InputActionReference NavigationScheme;



        /// <summary>
        /// Initialize default selectables state
        /// </summary>
        /// 
        public void Init()
        {
            if (InputManager.instance != null)
            {
                NavigationScheme = InputManager.instance.UI_NavigationScheme;
            }
            else
            {
                Debug.LogWarning("InputManager was not initialized...");
            }

            foreach (var sel in _selectables)
            {
                _defaultScales.TryAdd(sel, sel.gameObject.transform.localScale);
            }
            _firstSelected = _selectables[0];
        }

        void OnEnable()
        {
            if (NavigationScheme != null)
            {
                NavigationScheme.action.performed += OnNavigate;
            }
            foreach (var sel in _selectables)
            {
                if (_defaultScales.TryGetValue(sel, out var default_scale))
                {
                    sel.transform.localScale = default_scale;
                }

            }
            StartCoroutine(SelectFirstAfterDelay());
        }

        void OnDisable()
        {
            if (NavigationScheme != null)
            {
                NavigationScheme.action.performed += OnNavigate;
            }
            _scaleUpTween.Kill(true);
            _scaleDownTween.Kill(true);
        }
        /// <summary>
        /// Enable ui behaviour...
        /// </summary>
        public void EnableBehaviour()
        {
            if (NavigationScheme != null)
            {
                NavigationScheme.action.performed += OnNavigate;
            }

            foreach (var sel in _selectables)
            {
                AddListeners(sel);
            }
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Disable ui behaviour...
        /// </summary>
        public void DisableBehaviour()
        {
            foreach (var sel in _selectables)
            {
                RemoveListeners(sel);
            }
            gameObject.SetActive(false);
        }


        public virtual IEnumerator SelectFirstAfterDelay()
        {
            yield return null;
            if (_firstSelected != null)
                EventSystem.current.SetSelectedGameObject(_firstSelected.gameObject);
        }

        public virtual void AddSelectable(Selectable selectable)
        {
            _selectables.Add(selectable);
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
            PointerEventData pointerEventData = eventData as PointerEventData;
            if (pointerEventData != null)
            {
                pointerEventData.selectedObject = null;
            }
        }

        protected virtual void OnPointerEnter(BaseEventData eventData)
        {
            PointerEventData pointerEventData = eventData as PointerEventData;

            var item = pointerEventData.pointerEnter;
            var sel = item.GetComponentInParent<Selectable>();
            if (sel == null)
            {
                sel = item.GetComponentInChildren<Selectable>();
            }
            pointerEventData.selectedObject = sel.gameObject;
        }

        protected virtual void OnDeselect(BaseEventData eventData)
        {
            Selectable selectable = eventData.selectedObject.GetComponent<Selectable>();
            _scaleDownTween = eventData.selectedObject.transform.DOScale(_defaultScales[selectable], _scaleTransitionTime);
        }

        protected virtual void OnSelect(BaseEventData eventData)
        {
            _lastSelected = eventData.selectedObject.GetComponent<Selectable>();
            Vector3 newScale = eventData.selectedObject.transform.localScale * _scaleMultiplier;
            _scaleUpTween = eventData.selectedObject.transform.DOScale(newScale, _scaleTransitionTime);
        }



        protected virtual void OnNavigate(InputAction.CallbackContext context)
        {
            if (EventSystem.current.currentSelectedGameObject == null && _lastSelected != null)
            {
                EventSystem.current.SetSelectedGameObject(_lastSelected.gameObject);
            }
        }
    }
}
