using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

namespace TowerDefense
{
    /// <summary>
    /// Генератор волны противников.
    /// </summary>
    public class EnemiesSpawner : Spawner
    {

        /// <summary>
        /// Ппрефаб противника для генерации.
        /// </summary>
        [Header("Spawn Enemies Wave")]
        [SerializeField] internal Entity _enemyPrefab;

        /// <summary>
        /// Массив свойств сущностей для генерации.
        /// </summary>
        [SerializeField] private EnemyProperties[] _enemiesProperties;

        /// <summary>
        /// Команда волны пешеходов, которые создаёт генератор.
        /// </summary>
        [SerializeField] private int _waveTeam;
        
        /// <summary>
        /// Тип поведения ИИ новых пешеходов.
        /// </summary>
        [SerializeField] private AiBehaviour _behaviour;

        /// <summary>
        /// Массив точек маршрута патрулирования.
        /// </summary>
        [SerializeField] private Transform[] _patrolRoute;

        /// <summary>
        /// Точность выхода на точку патрулирования после чего пешеход нацелится на следующую точку.
        /// </summary>
        [SerializeField] private float _patrolRoutePrecision;
        
        /// <summary>
        /// Размещение пешехода на игровом поле.
        /// </summary>
        protected override GameObject GenerateSpawnedEntity()
        {
            //создать пешехода
            GameObject entity = Instantiate(_enemyPrefab.gameObject);
            //задать команду пешехода
            Walker walker = entity.GetComponent<Walker>();
            walker.TeamId = _waveTeam;
            AiWalkerController controller = entity.GetComponent<AiWalkerController>();
            //настроить поведение пешехода
            switch (_behaviour)
            {
                //case AiBehaviour.ZonePatrol:
                //    controller.SetPatrolZone(_zone);
                //    break;
                case AiBehaviour.RoutePatrolOnward:
                case AiBehaviour.RoutePatrolBackward:
                    controller.SetPatrolRoute(_patrolRoute, _patrolRoutePrecision, _behaviour);
                    break;
                default:
                    controller.SetBehaviourNone();
                    break;
            }

            //применение свойств врагу
            if (entity.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.Use(_enemiesProperties[Random.Range(0, _enemiesProperties.Length)]);
            }

            return entity;
        }
    }
}
