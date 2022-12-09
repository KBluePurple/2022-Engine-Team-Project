using AchromaticDev.Util.Pooling;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public AttackSo attackSo;
    public AttackSo heavyAttackSo;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform attackParent;

    private float _chargeGage;

    private float _timer;

    private void Start()
    {
        InGameUI.Instance.UpdateChargeGage(_chargeGage, heavyAttackSo.chargeGage);
    }

    public void Attack()
    {
        if (!(_timer <= 0)) return;

        GameObject attack = PoolManager.Instantiate(attackSo.prefab, attackPoint.position, attackPoint.rotation);
        attack.transform.SetParent(attackParent);
        _timer = attackSo.cooldown;
    }

    public void HeavyAttack()
    {
        if (_chargeGage < heavyAttackSo.chargeGage) return;

        GameObject attack = PoolManager.Instantiate(heavyAttackSo.prefab, attackPoint.position, attackPoint.rotation);
        attack.transform.SetParent(attackParent);
        _chargeGage = 0;
        InGameUI.Instance.UpdateChargeGage(_chargeGage, heavyAttackSo.chargeGage);
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
    }

    public void ChargeGage(int gage)
    {
        _chargeGage += gage;
        InGameUI.Instance.UpdateChargeGage(_chargeGage, heavyAttackSo.chargeGage);
    }
}