using UnityEngine;

namespace Skill
{
    [CreateAssetMenu(fileName = "Dash", menuName = "Skill/Dash")]
    public class DashSkill : SkillBase
    {
        protected override void OnUseSkill(Player player)
        {
            player.Dash();
        }
    }
}