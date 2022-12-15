using UnityEngine;

namespace Skill
{
    [CreateAssetMenu(fileName = "Heal", menuName = "Skill/Heal")]
    public class HealSkill : SkillBase
    {
        protected override void OnUseSkill(Player player)
        {
            player.Heal();
        }
    }
}