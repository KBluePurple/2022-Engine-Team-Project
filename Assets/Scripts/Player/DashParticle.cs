using UnityEngine;
using KBluePurple.Util;

public class DashParticle : MonoBehaviour
{
    private float _lifeTime = 3f;
    private float _nowTime = 0f;

    private void Update()
    {
        _nowTime += Time.deltaTime;
        if (_nowTime >= _lifeTime)
            DestroyAction();
    }

    private void DestroyAction()
    {
        _nowTime = 0;
        PoolManager.Destroy(gameObject);
    }
}