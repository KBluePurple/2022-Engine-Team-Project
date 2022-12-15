using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, HitAble
{
    private int maxHp = 100;
    private int nowHp;
    private int defence = 0;

    private HpBar hpBar;

    private void Awake()
    {
        hpBar = GetComponentInChildren<HpBar>();
        nowHp = maxHp;
    }

    [ContextMenu("АјАн")]
    public void Test()
    {
        Hit(10);
    }
    
    public void Hit(int damage)
    {
        nowHp -= damage - defence;
        hpBar.ChangeHp((float)nowHp / maxHp);
    }
}
