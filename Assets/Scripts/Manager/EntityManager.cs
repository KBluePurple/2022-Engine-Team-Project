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
        [SerializeField] private List<Entity> entities = new();
        public IReadOnlyList<Entity> Entities => entities;

        public override void Awake()
        {
            InitEntity();
        }

        private void InitEntity()
        {
            var sceneEntities = FindObjectsOfType<Entity>();
            foreach (var entity in sceneEntities)
            {
                entity.Initialize(Guid.NewGuid().ToString());
            }
            entities.AddRange(sceneEntities);
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
            entities.Add(entityComponent);
        }
        
        public void DestroyEntity(Entity entity)
        {
            entities.Remove(entity);
            PoolManager.Destroy(entity.gameObject);
        }
        
        public Entity GetEntity(string id)
        {
            return entities.FirstOrDefault(entity => entity.Id == id);
        }
    }
}