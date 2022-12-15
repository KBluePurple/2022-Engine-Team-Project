using UnityEngine;

namespace Skill
{
    [CreateAssetMenu(fileName = "Bomb", menuName = "Skill/Bomb")]
    public class BombSkill : SkillBase
    {
        protected override void OnUseSkill(Player player)
        {
            player.Bomb();
        }
    }
}