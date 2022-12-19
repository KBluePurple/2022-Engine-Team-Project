using System;
using Manager;
using UnityEngine;

namespace Skill
{
    [Serializable]
    public abstract class SkillBase : ScriptableObject
    {
        public Action OnSkillUnlocked = delegate { };
        
        public Sprite skillImage;
        public float coolTime;
        public float openTime = 30f;
        [NonSerialized] public float CoolTimeLeft;
        [NonSerialized] public bool IsUnlock;

        public void UseSkill(Player player)
        {
            if (!IsUnlock) return;
            if (!(CoolTimeLeft <= 0)) return;
            
            CoolTimeLeft = coolTime;
            OnUseSkill(player);
        }

        protected abstract void OnUseSkill(Player player);

        public void Unlock()
        {
            IsUnlock = true;
            OnSkillUnlocked?.Invoke();
        }

        public void Update()
        {
            if (GameManager.Instance.PlayTime < openTime) return;
            if (IsUnlock) return;
            
            Unlock();
        }
    }
}