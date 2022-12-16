using System;
using DG.Tweening;
using Skill;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SkillPanel : MonoBehaviour
    {
        [SerializeField] private Image skillImage;
        [SerializeField] private Image lockImage;
        [SerializeField] private Image coolTimeImage;
        
        private SkillBase _skill;
        private bool _isLock = true;

        private Color _tempColor;

        private void Awake()
        {
            _tempColor = coolTimeImage.color;
            _tempColor.a = 0.6f;
        }

        public void SetSkill(SkillBase skill)
        {
            _skill = skill;
            skillImage.sprite = skill.skillImage;
            lockImage.gameObject.SetActive(!skill.IsUnlock);
            
            _skill.OnSkillUnlocked += OnSkillUnlocked;
        }

        private void OnSkillUnlocked()
        {
            (lockImage.transform as RectTransform).DOAnchorPosY(-200, 1f)
                .SetEase(Ease.InOutBack);
            lockImage.DOFade(0, 0.5f)
                .OnComplete(() => lockImage.gameObject.SetActive(false));
            coolTimeImage.DOFade(0, 0.5f)
                .OnComplete(() =>
                {
                    _isLock = false;
                });
        }

        private void Update()
        {
            if (_isLock) return;
            coolTimeImage.color = _tempColor;
            coolTimeImage.fillAmount = _skill.CoolTimeLeft / _skill.coolTime;
        }
    }
}