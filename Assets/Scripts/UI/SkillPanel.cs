using DG.Tweening;
using Manager;
using Skill;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SkillPanel : MonoBehaviour
    {
        [SerializeField] private Image skillImage;
        [SerializeField] private Image coolTimeImage;
        [SerializeField] private TextMeshProUGUI lockTimeText;

        [SerializeField] private Image activeTimeImage;

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
            lockTimeText.gameObject.SetActive(!skill.IsUnlock);

            _skill.OnSkillUnlocked += OnSkillUnlocked;
        }

        private void OnSkillUnlocked()
        {
            (lockTimeText.transform as RectTransform).DOAnchorPosY(-200, 1f)
                .SetEase(Ease.InOutBack);
            lockTimeText.DOFade(0, 0.5f)
                .OnComplete(() => lockTimeText.gameObject.SetActive(false));
            coolTimeImage.DOFade(0, 0.5f)
                .OnComplete(() => { _isLock = false; });
        }

        private void Update()
        {
            lockTimeText.text = Mathf.Max(0, _skill.unlockTime - GameManager.Instance.GameTime).ToString("F1");
            if (_isLock) return;
            coolTimeImage.color = _tempColor;

            if (_skill is StealthSkill stealthSkill)
            {
                if (0 < stealthSkill.ActiveTimeRemaining)
                {
                    activeTimeImage.gameObject.SetActive(true);
                    activeTimeImage.fillAmount = stealthSkill.ActiveTimeRemaining / stealthSkill.activeTime;
                }
                else
                {
                    activeTimeImage.gameObject.SetActive(false);
                    coolTimeImage.fillAmount = _skill.CoolTimeLeft / _skill.coolTime;
                }
            }
            else
            {
                coolTimeImage.fillAmount = _skill.CoolTimeLeft / _skill.coolTime;
            }
        }
        
        private void OnDestroy()
        {
            _skill.OnSkillUnlocked -= OnSkillUnlocked;
        }
    }
}