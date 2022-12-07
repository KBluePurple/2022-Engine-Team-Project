using AchromaticDev.Util.Pooling;
using UnityEngine;
public class PlayerAttackController : MonoBehaviour
{
    public AttackSO AttackSO
    {
        get => attackSO;
        set => attackSO = value;
    }

    [SerializeField] private AttackSO attackSO;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform bulletParent;

    private float _timer;

    public void Attack()
    {
        if (_timer <= 0)
        {
            GameObject bullet = PoolManager.Instantiate(attackSO.prefab, attackPoint.position, attackPoint.rotation);
            bullet.transform.SetParent(bulletParent);
            _timer = attackSO.cooldown;
        }
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
    }
}