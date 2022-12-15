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
        
        public void SetSkill(SkillBase skill)
        {
            _skill = skill;
            skillImage.sprite = skill.skillImage;
            lockImage.gameObject.SetActive(!skill.IsUnlock);
        }

        private void Update()
        {
            if (_skill == null) return;
            if (!_skill.IsUnlock) return;
            
            lockImage.gameObject.SetActive(false);
            coolTimeImage.fillAmount = _skill.CoolTimeLeft / _skill.coolTime;
        }
    }
}