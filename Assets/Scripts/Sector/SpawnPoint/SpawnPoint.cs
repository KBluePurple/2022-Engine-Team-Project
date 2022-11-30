using System.Collections;
using AchromaticDev.Util.Pooling;
using UnityEngine;

public class SpawnPoint<T> : MonoBehaviour where T : Entity
{
    [SerializeField] private bool isSpawned;
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject entityPrefab;

    private Sector _sector;

    public void Spawn(Sector sector)
    {
        Debug.Log(sector);
        _sector = sector;
        _sector.EnemyCount++;
        isSpawned = true;
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(spawnDelay);
        var enemy = PoolManager.Instantiate(entityPrefab, transform.position, Quaternion.identity);
        enemy.GetComponent<LivingEntity>().OnDeath.AddListener(() => _sector.EnemyCount--);
    }
}