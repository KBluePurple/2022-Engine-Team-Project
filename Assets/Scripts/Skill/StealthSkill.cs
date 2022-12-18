using UnityEngine;

namespace Skill
{
    [CreateAssetMenu(fileName = "Stealth", menuName = "Skill/Stealth")]
    public class StealthSkill : SkillBase
    {
        protected override void OnUseSkill(Player player)
        {
            player.Stealth();
        }
    }
}