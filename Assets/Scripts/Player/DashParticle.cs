using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBluePurple.Util;

public class DashParticle : MonoBehaviour
{
    private float lifeTime = 0.5f;
    private float nowTime = 0f;

    private void Update()
    {
        nowTime += Time.deltaTime;
        if (nowTime >= lifeTime)
            DestroyAction();
    }

    private void DestroyAction()
    {
        nowTime = 0;
        PoolManager.Destroy(gameObject);
    }

}
