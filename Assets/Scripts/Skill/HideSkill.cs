using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    [CreateAssetMenu(fileName = "Hide", menuName = "Skill/Hide")]
    public class HideSkill : SkillBase
    {
        protected override void OnUseSkill(Player player)
        {
            player.Hide();
        }
    }
}

