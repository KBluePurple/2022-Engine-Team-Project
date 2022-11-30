using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sector : MonoBehaviour
{
    [SerializeField] private List<Sector> nextSectors = new();
    [SerializeField] private List<SectorDoor> doors = new();
    [SerializeField] private List<SpawnPoint<Enemy>> enemiesSpawnPoints = new();

    public bool IsCleared => _enemyCount == 0 && _isActivated;

    private int _enemyCount;
    private bool _isActivated;

    public int EnemyCount
    {
        get => _enemyCount;
        set
        {
            _enemyCount = value;
            if (_enemyCount == 0)
            {
                ActivateNextSectors();
            }
        }
    }

    private void Open()
    {
        _isActivated = true;
        foreach (var door in doors)
        {
            door.Open();
        }
    }

    protected void ActivateNextSectors()
    {
        nextSectors.ToList().ForEach(sector => sector.Open());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isActivated || !IsCleared) return;

        foreach (var enemiesSpawnPoint in enemiesSpawnPoints)
        {
            enemiesSpawnPoint.Spawn(this);
        }
    }
}