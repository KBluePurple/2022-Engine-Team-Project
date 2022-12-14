using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletEmitter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public abstract void Fire();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }
}