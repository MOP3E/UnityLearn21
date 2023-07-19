using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TowerDefense
{
    /// <summary>
    /// Генератор сущностей.
    /// </summary>
    public class EntitySpawner : Spawner
    {
        /// <summary>
        /// Массив префабов сущностей для генерации.
        /// </summary>
        [Header("Entity Spawner")]
        [SerializeField] internal Entity[] _entityPrefabs;

        /// <summary>
        /// Счётчик времени генерации.
        /// </summary>
        private float _spawnTimer;

        protected override GameObject GenerateSpawnedEntity()
        {
            int index = UnityEngine.Random.Range(0, _entityPrefabs.Length);
            GameObject entity = Instantiate(_entityPrefabs[index].gameObject);
            return entity;
        }
    }
}
