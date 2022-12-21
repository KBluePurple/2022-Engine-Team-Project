using System.Collections;
using KBluePurple.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager
{
    public class BulletManager : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletParent;
        [SerializeField] private Transform player;

        public void SpawnBullet(Vector3 position, Quaternion rotation)
        {
            var positionOffset = position + player.position;
            positionOffset.y = -1;
            var bullet = PoolManager.Instantiate(bulletPrefab, position + positionOffset, rotation);
            bullet.transform.SetParent(bulletParent);
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

        // 원신 뇌음의 권현 양쪽에서 벽 다가오는거
        public void SpawnBulletWall(int width, int height, int randWall) 
        {
            for (int i = -(height/2); i < height - height/2; i++)
            {
                for (int j = -(width/2); j < width -width/2; j++)
                {
                    Vector3 fPos = new Vector3(j, i, 5);
                    Vector3 bPos = new Vector3(j, i, -5);
                    Vector3 rPos = new Vector3(5, i, j);
                    Vector3 lPos = new Vector3(-5, i, j);

                    if (randWall == 1)
                    {
                        SpawnBullet(lPos, Quaternion.identity);
                        SpawnBullet(rPos, Quaternion.identity);
                    }
                    else
                    {
                        SpawnBullet(fPos, Quaternion.identity);
                        SpawnBullet(bPos, Quaternion.identity);
                    }
                }
            }
        }

        public void SpawnBulletCross(int width, int length)
        {
            for (int j = -(width/2); j < width -(width/2); j++)
            {
                for (int i = 0; i < length; i++)
                {
                    Vector3 fPos = new Vector3(j, 0, i + 5);
                    Vector3 bPos = new Vector3(j, 0, -i - 5);

                    Vector3 lPos = new Vector3(-i - 5, 0, j);
                    Vector3 rPos = new Vector3(i + 5, 0, j);

                    SpawnBullet(fPos, Quaternion.identity);
                    SpawnBullet(bPos, Quaternion.identity);
                    SpawnBullet(lPos, Quaternion.identity);
                    SpawnBullet(rPos, Quaternion.identity);
                }
            }
        }

        public void SpawnBulletThreeWall(int width, int height, int distance, int noWall) // 1, 2, 3, 4순서대로 위부터 시계방향
        {
            for (int i = -(height / 2); i < height - height / 2; i++)
            {
                for (int j = -(width / 2); j < width - width / 2; j++) // 아직 미완임
                {
                    Vector3 fPos = new Vector3(j, i,  distance);
                    Vector3 bPos = new Vector3(j, i, -distance);
                    Vector3 lPos = new Vector3(-distance, i, j);
                    Vector3 rPos = new Vector3( distance, i, j);

                    if (noWall != 1)
                        SpawnBullet(fPos, Quaternion.identity);
                    if (noWall != 2)
                        SpawnBullet(rPos, Quaternion.identity);
                    if (noWall != 3)
                        SpawnBullet(bPos, Quaternion.identity);
                    if (noWall != 4)
                        SpawnBullet(lPos, Quaternion.identity);
                }
            }
        }

        public void SpawnBulletNum()
        {

        }

        private void Start()
        {
            // code for debugging
            //StartCoroutine(RepeatSpawn());
        }

        public void StartSpawn()
        {
            StartCoroutine(RepeatSpawn());
        }
        
        private IEnumerator RepeatSpawn()
        {
            yield return new WaitForSeconds(3f);
            while (true)
            {

                int pattern = Random.Range(1, 5);

                switch(pattern)
                {
                    case 1:
                        int randThreeWall = Random.Range(1, 5);
                        SpawnBulletThreeWall(8, 3, 5, randThreeWall);
                        break;
                    case 2:
                        SpawnBulletCross(3, 5);
                        break;
                    case 3:
                        int randWall = Random.Range(1, 3);
                        SpawnBulletWall(15, 5, randWall);
                        break;
                    case 4:
                        SpawnBulletCircle(10, 10);
                        break;
                }
                //SpawnBulletThreeWall(8, 3, 5, 2);
                //SpawnBulletCross(3, 5);
                //SpawnBulletWall(15, 5);
                //SpawnBulletCircle(10, 10);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}