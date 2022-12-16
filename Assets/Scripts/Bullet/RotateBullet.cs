using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBluePurple.Util;

public class RotateBullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private float nowTime = 0;
    [SerializeField] private GameObject target;

    Vector3 targetVec;

    private void OnEnable()
    {
        target = GameObject.Find("Player");
        targetVec = target.transform.position - transform.position;
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        if (nowTime >= lifeTime)
            DestoryAction();

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void DestoryAction()
    {
        nowTime = 0;
        PoolManager.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        HitAble target = other.GetComponent<HitAble>();
        if (target != null)
        {
            target.Hit(damage);
            DestoryAction();
        }
    }
}