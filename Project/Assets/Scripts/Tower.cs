using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

namespace TowerDefense
{
    public class Tower : Destructible
    {
        /// <summary>
        /// Предпосмотр башни.
        /// </summary>
        [Header("Tower")]
        [SerializeField] private Sprite _previewImage;

        /// <summary>
        /// Предпосмотр башни.
        /// </summary>
        public Sprite PreviewImage => _previewImage;

        /// <summary>
        /// Скрипт управления башней.
        /// </summary>
        [SerializeField] private Controller _towerController;

        /// <summary>
        /// Скрипт управления башней.
        /// </summary>
        public Controller Controller
        {
            get => _towerController;
            set => _towerController = value;
        }

        /// <summary>
        /// Максимальное (и начальное) число очков жизни разрушаемого объекта.
        /// </summary>
        public int MaxHitpoints => _maxHitpoints;

        /// <summary>
        /// Орудие башни.
        /// </summary>
        [SerializeField] private Turret _turret;

        /// <summary>
        /// Орудие башни.
        /// </summary>
        public Turret Turret => _turret;

        /*
        /// <summary>
        /// Максимальная энергия башни.
        /// </summary>
        [SerializeField] private int _maxEnergy;

        /// <summary>
        /// Начальная энергия башни. Если не используется - поставить значение меньше 0.
        /// </summary>
        [SerializeField] private int _startEnergy;

        /// <summary>
        /// Восстановление энергии башни.
        /// </summary>
        [SerializeField] private int _energyRegenPerSecond;

        /// <summary>
        /// Максимальный боезапас башни.
        /// </summary>
        [SerializeField] private int _maxAmmo;

        /// <summary>
        /// Начальный боезапас башни. Если не используется - поставить значение меньше 0.
        /// </summary>
        [SerializeField] private int _startAmmo;

        /// <summary>
        /// Энергия основного оружия башни.
        /// </summary>
        private float _primaryEnergy;

        /// <summary>
        /// Боеприпасы вторичного оружия башни.
        /// </summary>
        private int _secondaryAmmo;
        */

        /// <summary>
        /// Start запускается перед первым кадром.
        /// </summary>
        protected override void Start()
        {
            base.Start();
            _indestrictible = true;
        }

        /// <summary>
        /// Update запускается каждый кадр.
        /// </summary>
        private void Update()
        {
            //обработка стрельбы из основного либо вторичного оружия
            if (_towerController != null)
            {
                if (_towerController.PrimaryButton > 0f || _towerController.SecondaryButton > 0f) Fire();
            }
        }

        /// <summary>
        /// FixedUpdate запускается с фиксированным периодом.
        /// </summary>
        private void FixedUpdate()
        {
            //обработка движения
            MovementControl();
            //восстановление энергии башни
            //EnergyRegen();
        }

        /// <summary>
        /// Обработка движения башни.
        /// </summary>
        private void MovementControl()
        {
            //башня просто поворачивается в заданном направлении
            //if(_towerController != null) transform.eulerAngles = new Vector3(0, 0, _towerController.RotationAngle);
        }

        /// <summary>
        /// Стрельба из турели башни.
        /// </summary>
        public void Fire()
        {
            if (_towerController != null) Turret.transform.rotation = _towerController.Rotation;
            _turret.Fire();
        }

        /*
        /// <summary>
        /// Добавление энергии башне.
        /// </summary>
        public void AddEnergy(int energy)
        {
            _primaryEnergy += energy;
            if (_primaryEnergy > _maxEnergy) _primaryEnergy = _maxEnergy;
        }

        /// <summary>
        /// Добавление патронов башне.
        /// </summary>
        public void AddAmmo(int ammo)
        {
            _secondaryAmmo += ammo;
            if (_secondaryAmmo > _maxAmmo) _secondaryAmmo = _maxAmmo;
        }

        /// <summary>
        /// Регенрация энергии.
        /// </summary>
        private void EnergyRegen()
        {
            _primaryEnergy += (float)_energyRegenPerSecond * Time.fixedDeltaTime;
            if (_primaryEnergy > _maxEnergy) _primaryEnergy = _maxEnergy;
        }
        */

        /// <summary>
        /// Расходование патронов.
        /// </summary>
        public bool DrawAmmo(int count)
        {
            //TODO: Заглушка, переписать в соответствии с новой концепцией. Используется в Turret.
            return true;
        }

        /// <summary>
        /// Расходование энергии.
        /// </summary>
        public bool DrawEnergy(int count)
        {
            //TODO: Заглушка, переписать в соответствии с новой концепцией. Используется в Turret.
            return true;
        }

        /*
        /// <summary>
        /// Назначить новые свойства турелям башни.
        /// </summary>
        /// <param name="properties">Свойства, которые  нужно назначить.</param>
        public void AssignWeapon(TurretProperties properties)
        {
            foreach (Turret turret in _turrets)
            {
                turret.AssignLoadout(properties);
            }
        }
        */

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _turret.FiringRadius);
        }
    }
}
