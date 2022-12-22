using System;
using UnityEngine;

namespace Skill
{
    [CreateAssetMenu(fileName = "Stealth", menuName = "Skill/Stealth")]
    public class StealthSkill : SkillBase
    {
        public float activeTime;
        [NonSerialized] public float ActiveTimeRemaining;

        public override void Update()
        {
            if (IsUnlock)
            {
                if (0 < ActiveTimeRemaining)
                {
                    ActiveTimeRemaining -= Time.deltaTime;
                }
                else
                {
                    base.Update();
                }
            }
            else
            {
                base.Update();
            }
        }

        protected override void OnUseSkill(Player player)
        {
            ActiveTimeRemaining = activeTime;
            player.Stealth(ActiveTimeRemaining);
            if (skillClip != null) Script.Manager.SoundManager.Instance.PlayEffect(skillClip);
        }
    }
}