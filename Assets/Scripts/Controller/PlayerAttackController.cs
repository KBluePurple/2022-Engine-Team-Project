using AchromaticDev.Util.Pooling;
using UnityEngine;
using UnityEngine.Serialization;
public class PlayerAttackController : MonoBehaviour
{
    public AttackSO AttackSo
    {
        get => attackSo;
        set => attackSo = value;
    }

    [FormerlySerializedAs("attackSO")]
    [SerializeField] private AttackSO attackSo;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform bulletParent;

    private float _timer;

    public void Attack()
    {
        if (!(_timer <= 0)) return;

        GameObject bullet = PoolManager.Instantiate(attackSo.prefab, attackPoint.position, attackPoint.rotation);
        bullet.transform.SetParent(bulletParent);
        _timer = attackSo.cooldown;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
    }
}