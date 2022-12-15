using System;
using UnityEngine;

namespace Skill
{
    [Serializable]
    public abstract class SkillBase : ScriptableObject
    {
        public Sprite skillImage;
        public float coolTime;
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
        }
    }
}