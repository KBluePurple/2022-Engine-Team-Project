using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatus : MonoBehaviour, HitAble
{
    public UnityEvent hitEvent;

    private int maxHp = 100;
    private int nowHp;
    private int defence = 0;

    private float invincivilityTime = 0.25f;
    private bool isInvincivility = false;

    private HpBar hpBar;

    private void Awake()
    {
        hpBar = transform.Find("Health Bar Canvas").GetComponent<HpBar>();
        nowHp = maxHp;
    }

    IEnumerator InvincivilityCheck()
    {
        isInvincivility = true;
        yield return new WaitForSeconds(invincivilityTime);
        isInvincivility = false;
    }


    public void Test()
    {
        Hit(10);
    }
    
    public void Hit(int damage)
    {
        if (isInvincivility == true) return;
        nowHp -= damage - defence;
        hitEvent?.Invoke();
        hpBar.ChangeHp((float)nowHp / maxHp);
    }
}
