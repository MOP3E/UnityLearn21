using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public enum SpawnMode
    {
        /// <summary>
        /// Генерация количества для спавна сразу при старте.
        /// </summary>
        Start,
        /// <summary>
        /// Постоянная генерация порциями, равными количеству для спавна.
        /// </summary>
        Loop,
        /// <summary>
        /// Генерация волны порциями, равными количеству для спавна, до исчерпания размера волны.
        /// </summary>
        Wave
    }

    /// <summary>
    /// Генератор всего.
    /// </summary>
    public abstract class Spawner : MonoBehaviour
    {
        /// <summary>
        /// Зона спавна.
        /// </summary>
        [Header("Spawner")]
        [SerializeField] protected CircleArea _spawnArea;
        
        /// <summary>
        /// Режим генерации.
        /// </summary>
        [SerializeField] protected SpawnMode _spawnMode;

        /// <summary>
        /// Количество одновременно генерируемых объектов.
        /// </summary>
        [SerializeField] protected int _countSpawns;

        /// <summary>
        /// Количество одновременно генерируемых объектов.
        /// </summary>
        [SerializeField] protected int _waveSize;

        /// <summary>
        /// Сколько осталось заспавнить противнкиов.
        /// </summary>
        private int _waveRemainder;

        /// <summary>
        /// Период генерации.
        /// </summary>
        [SerializeField] protected float _spawnPeriod;
        
        /// <summary>
        /// Счётчик времени генерации.
        /// </summary>
        private float _spawnTimer;

        /// <summary>
        /// Размещённый объект.
        /// </summary>
        protected abstract GameObject GenerateSpawnedEntity();

        internal virtual void Awake()
        {
            _waveRemainder = _waveSize;
        }

        internal virtual void Start()
        {
            if (_spawnMode == SpawnMode.Start)
            {
                //генерация сущности при старте
                SpawnEntities();
            }

            _spawnTimer = _spawnPeriod;
        }

        internal void Update()
        {
            switch (_spawnMode)
            {
                case SpawnMode.Start:
                case SpawnMode.Wave when _waveRemainder <= 0:
                    return;
            }

            if (_spawnTimer > 0) _spawnTimer -= Time.deltaTime;
            if (!(_spawnTimer <= 0)) return;
            
            SpawnEntities();
            _spawnTimer = _spawnPeriod;
        }

        internal virtual void SpawnEntities()
        {
            if(_spawnMode == SpawnMode.Wave && _waveRemainder <= 0) return;
            for (int i = 0; i < _countSpawns; i++)
            {
                GameObject entity = GenerateSpawnedEntity();
                entity.transform.position = _spawnArea.GetRandomInsideArea();
                if (_spawnMode != SpawnMode.Wave) continue;

                _waveRemainder--;
                if (_waveRemainder <= 0) return;
            }
        }
    }
}
