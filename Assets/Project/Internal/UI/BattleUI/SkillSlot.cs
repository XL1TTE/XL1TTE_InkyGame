using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Internal.SkillsSystem;
using Project.Internal.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Internal.UI
{
    public class SkillSlot : MonoBehaviour
    {
        [HideInInspector] public BaseSkill AttachedSkill;

        #region Visuals
        [SerializeField] public Image SkillIconImage;

        protected Tween _SkillscaleUpTween;
        protected Tween _SkillscaleDownTween;

        protected Color _defaultSkillColor;
        [SerializeField] protected Color _onSelectedSkillColor;

        protected Vector3 _defaultSkillScale;

        protected bool IsLocked = false;

        #endregion

        void Start()
        {
            _defaultSkillColor = SkillIconImage.color;
            _defaultSkillScale = gameObject.transform.localScale;
        }

        void OnDisable()
        {
            _SkillscaleUpTween.Kill(true);
            _SkillscaleDownTween.Kill(true);
        }

        void OnEnable()
        {
            if (_defaultSkillScale != Vector3.zero)
                gameObject.transform.localScale = _defaultSkillScale;
        }

        #region Animations


        public void LockAnimationState(bool islock)
        {
            IsLocked = islock;
        }
        public void DOSelectAnimation()
        {
            if (!IsLocked)
            {
                // SCALE ANIM
                var TargetScale = _defaultSkillScale * 1.1f;
                _SkillscaleUpTween = gameObject.transform.DOScale(TargetScale, 0.25f);

                // COLOR ANIM
                SkillIconImage.color = _onSelectedSkillColor;
            }

        }

        public void DODeselectAnimation()
        {
            if (!IsLocked)
            {
                _SkillscaleDownTween = gameObject.transform.DOScale(_defaultSkillScale, 0.25f);
                SkillIconImage.color = _defaultSkillColor;
            }

        }

        #endregion
    }
}
