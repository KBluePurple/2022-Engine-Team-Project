using System;
using System.Collections;
using KBluePurple.Util;
using UnityEngine;

namespace Manager
{
    public class BulletManager : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform player;
        
        public void SpawnBullet(Vector3 position, Quaternion rotation)
        {
            var positionOffset = position + player.position;
            positionOffset.y = -1;
            PoolManager.Instantiate(bulletPrefab, position + positionOffset, rotation);
        }
        
        public void SpawnBulletCircle(int amount, float radius)
        {
            for (int i = 0; i < amount; i++)
            {
                float angle = i * Mathf.PI * 2 / amount;
                Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                Quaternion rot = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
                SpawnBullet(pos, rot);
            }
        }

        private void Start()
        {
            // code for debugging
            StartCoroutine(RepeatSpawn());
        }
        
        private IEnumerator RepeatSpawn()
        {
            while (true)
            {
                SpawnBulletCircle(10, 10);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}