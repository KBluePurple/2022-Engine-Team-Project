using System;
using System.Collections.Generic;
using System.Linq;
using AchromaticDev.Util.Pooling;
using Script.Util;
using UnityEngine;

namespace Script.Manager
{
    public class EntityManager : MonoSingleton<EntityManager>
    {
        private readonly List<Entity> _entities = new();
        public IReadOnlyList<Entity> Entities => _entities;

        public void InitEntity()
        {
            var entities = FindObjectsOfType<MonoBehaviour>().OfType<Entity>();
            foreach (var entity in _entities)
            {
                entity.Initialize(Guid.NewGuid().ToString());
            }
            _entities.AddRange(entities);
        }
        
        public void SpawnEntity(EntityType entityType, Vector3 position)
        {
            var entity = PoolManager.Instantiate(entityType.prefab, position, Quaternion.identity);
            var entityComponent = entity.GetComponent<Entity>();
            if (entityComponent == null)
            {
                Debug.LogError($"Entity {entityType} does not have Entity component");
                return;
            }
            entityComponent.Initialize(Guid.NewGuid().ToString());
            _entities.Add(entityComponent);
        }
        
        public void DestroyEntity(Entity entity)
        {
            _entities.Remove(entity);
            PoolManager.Destroy(entity.gameObject);
        }
        
        public Entity GetEntity(string id)
        {
            return _entities.FirstOrDefault(entity => entity.Id == id);
        }
    }
}