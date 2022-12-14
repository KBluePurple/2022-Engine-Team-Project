using System.Collections;
using System.Collections.Generic;
using AchromaticDev.Util.Pooling;
using UnityEngine;

public class AutoPool : MonoBehaviour
{
    void Update()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) > 100)
        {
            PoolManager.Destroy(gameObject);
        }
    }
}