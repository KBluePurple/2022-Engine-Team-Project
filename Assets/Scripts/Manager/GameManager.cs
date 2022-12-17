using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBluePurple.Util;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    Camera cam;

    float x;
    float rand;
    float z;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        StartCoroutine(BulletMake());
    }

    private IEnumerator BulletMake()
    {
        while (true)
        { //
            Te();
            yield return new WaitForSeconds(1f);
        }
    }

    private void Te()
    {
        int a = Random.Range(0, 2);
        switch (a)
        {
            case 0: // 위 아래에서
                x = Random.Range(-21f, 21f);
                rand = Random.Range(0, 2);
                z = rand == 0 ? -12 : 12;
                break;

            case 1: // 좌 우에서
                z = Random.Range(-12f, 12f);
                rand = Random.Range(0, 2);
                x = rand == 0 ? -21 : 21;
                break;
        }

        PoolManager.Instantiate(bullet, new Vector3(x, -1, z), Quaternion.Euler(-90, 0, 0));
    }
}