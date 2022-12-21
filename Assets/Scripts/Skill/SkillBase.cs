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
        public float unlockTime = 30f;
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

        public virtual void Update()
        {
            if (IsUnlock)
            {
                CoolTimeLeft -= Time.deltaTime;
            }
            else if (unlockTime <= GameManager.Instance.GameTime && !IsUnlock)
            {
                Unlock();
            }
        }

        public void Init()
        {
            CoolTimeLeft = 0;
            IsUnlock = false;
        }
    }
}