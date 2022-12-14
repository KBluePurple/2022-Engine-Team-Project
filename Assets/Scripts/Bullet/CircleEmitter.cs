using AchromaticDev.Util.Pooling;
using UnityEngine;

public class CircleEmitter : BulletEmitter
{
    public float radius = 10f;
    public int count = 100;
    public float bulletSpeed = 1f;

    public override void Fire()
    {
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2 / count;
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            GameObject bullet = PoolManager.Instantiate(bulletPrefab, transform.position + direction * radius, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
        }
    }
}