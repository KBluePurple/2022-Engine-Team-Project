using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AchromaticDev.Util.Pooling;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        HitAble target = other.GetComponent<HitAble>();
        if (target != null)
        {
            target.Hit(damage);
            PoolManager.Destroy(gameObject);
        }
    }
}
