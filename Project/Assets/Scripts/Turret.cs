using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    /// <summary>
    /// Турель. Стреляет снарядами.
    /// </summary>
    public class Turret : MonoBehaviour
    {
        /// <summary>
        /// Тип турели.
        /// </summary>
        public TurretType Type => _properties.Type;

        /// <summary>
        /// Скорость снаряда.
        /// </summary>
        public float ProjectileVelocity => _properties.ProjectilePrefab.Velocity;

        /// <summary>
        /// Радиус поражения.
        /// </summary>
        public float FiringRadius => _properties.ProjectilePrefab.LifeRadius;

        /// <summary>
        /// Свойства турели.
        /// </summary>
        [SerializeField] private TurretProperties _properties;

        /// <summary>
        /// Таймер перезарядки турели.
        /// </summary>
        private float _reloadTimer;

        /// <summary>
        /// Турель может выстрелить.
        /// </summary>
        public bool CanFire => _reloadTimer <= 0;

        /// <summary>
        /// Космический корабль.
        /// </summary>
        private Tower _tower;

        private Collider2D _towerCollider;

        /// <summary>
        /// Start запускается перед первым кадром.
        /// </summary>
        private void Start()
        {
            Transform root = transform.root;
            _tower = root.GetComponent<Tower>();
            _towerCollider = root.GetComponentInChildren<Collider2D>();
        }

        /// <summary>
        /// Update запускается каждый кадр.
        /// </summary>
        private void Update()
        {
            if(_reloadTimer > 0) _reloadTimer -= Time.deltaTime;
        }

        /// <summary>
        /// Стрельба из турели.
        /// </summary>
        public void Fire()
        {
            if (_properties == null || _reloadTimer > 0) return;

            //попытаться выстрелить
            switch (_properties.Type)
            {
                case TurretType.Primary:
                    if(!_tower.DrawEnergy(_properties.EnergyUsage)) return;
                    break;
                case TurretType.Secondary:
                    if (!_tower.DrawAmmo(_properties.AmmoUsage)) return;
                    break;
            }

            //создать снаряд
            Projectile projectile = Instantiate(_properties.ProjectilePrefab).GetComponent<Projectile>();

            ////запретить столкновения между снарядом и башней
            //Collider2D projectileCollider = projectile.transform.root.GetComponentInChildren<Collider2D>();
            //if(projectileCollider != null) Physics2D.IgnoreCollision(_towerCollider, projectileCollider);

            //задать позициюи направление движения снаряда
            Transform projectileTransform = projectile.transform;
            Transform myTransform = transform;
            projectileTransform.position = myTransform.position;
            projectileTransform.up = myTransform.up;

            //запретить столкновения между снарядом и кораблём
            projectile.ParentDestructible = _tower;

            //перезапустить таймер перезарядки
            _reloadTimer = 60 / _properties.FireRate;

            //todo воспроизвести звук выстрела (домашнее задание)
        }

        /// <summary>
        /// Загрузка других свойств турели.
        /// </summary>
        public void AssignLoadout(TurretProperties properties)
        {
            //проверка на соответствие типа полученных свойств с типом турели
            if(_properties.Type != properties.Type) return;

            _reloadTimer = 0;
            _properties = properties;
        }
    }
}
