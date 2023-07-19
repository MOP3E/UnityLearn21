using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

namespace TowerDefense
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Walker : Destructible
    {
        /// <summary>
        /// Предпосмотр пешехода.
        /// </summary>
        [Header("Walker")]
        [SerializeField] private Sprite _previewImage;

        /// <summary>
        /// Предпосмотр пешехода.
        /// </summary>
        public Sprite PreviewImage => _previewImage;

        /// <summary>
        /// Скрипт управления кораблём.
        /// </summary>
        [SerializeField] private Controller _movementController;

        /// <summary>
        /// Скрипт управления кораблём.
        /// </summary>
        public Controller Controller
        {
            get => _movementController;
            set => _movementController = value;
        }

        /*
        /// <summary>
        /// Масса.
        /// </summary>
        [SerializeField] private float _mass;
        */

        /// <summary>
        /// Максимальное (и начальное) число очков жизни разрушаемого объекта.
        /// </summary>
        public int MaxHitpoints => _maxHitpoints;

        /// <summary>
        /// Максимальная линейная скорость.
        /// </summary>
        [SerializeField] private float _maxVelocity;

        /// <summary>
        /// Текущая векторная скорость.
        /// </summary>
        public Vector2 Velocity => new Vector2(Controller.HorisontalAxis, Controller.VerticalAxis).normalized * _maxVelocity;

        /// <summary>
        /// Сила бонуса скорости.
        /// </summary>
        public float VelocityPowerup { get; set; }

        /// <summary>
        /// Ссылка на физическое тело пешехода.
        /// </summary>
        private Rigidbody2D _myRigidbody;

        /// <summary>
        /// Ссылка на рендерер спрайта пешехода.
        /// </summary>
        private SpriteRenderer _mySpriteRenderer;

        /// <summary>
        /// Турели космического пешехода.
        /// </summary>
        [SerializeField] private Turret[] _turrets;

        /*
        /// <summary>
        /// Максимальная энергия космического пешехода.
        /// </summary>
        [SerializeField] private int _maxEnergy;

        /// <summary>
        /// Начальная энергия космического пешехода. Если не используется - поставить значение меньше 0.
        /// </summary>
        [SerializeField] private int _startEnergy;

        /// <summary>
        /// Восстановление энергии пешехода.
        /// </summary>
        [SerializeField] private int _energyRegenPerSecond;

        /// <summary>
        /// Максимальный боезапас космического пешехода.
        /// </summary>
        [SerializeField] private int _maxAmmo;

        /// <summary>
        /// Начальный боезапас космического пешехода. Если не используется - поставить значение меньше 0.
        /// </summary>
        [SerializeField] private int _startAmmo;

        /// <summary>
        /// Энергия основного оружия космического пешехода.
        /// </summary>
        private float _primaryEnergy;

        /// <summary>
        /// Боеприпасы вторичного оружия космического пешехода.
        /// </summary>
        private int _secondaryAmmo;
        */

        /// <summary>
        /// Start запускается перед первым кадром.
        /// </summary>
        protected override void Start()
        {
            _myRigidbody = GetComponent<Rigidbody2D>();
            _mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            //_myRigidbody.mass = _mass;
            //_myRigidbody.inertia = 1;

            //_primaryEnergy = _startEnergy >= 0 ? _startEnergy : _maxEnergy;
            //_secondaryAmmo = _startAmmo >= 0 ? _startAmmo : _maxAmmo;

            base.Start();
        }

        /// <summary>
        /// Update запускается каждый кадр.
        /// </summary>
        private void Update()
        {
            /*
            //обработка стрельбы из основного либо вторичного оружия
            if (_movementController != null)
            {
                //стрельба из основного оружия
                if (_movementController.PrimaryButton > 0f) Fire(TurretType.Primary);
                //стрельба из вторичного оружия
                if (_movementController.SecondaryButton > 0f) Fire(TurretType.Secondary);
            }
            */
        }

        /// <summary>
        /// FixedUpdate запускается с фиксированным перидом.
        /// </summary>
        private void FixedUpdate()
        {
            //обработка движения
            MovementControl();
            //восстановление энергии пешехода
            //EnergyRegen();
        }

        /// <summary>
        /// Обработка движения человека.
        /// </summary>
        private void MovementControl()
        {
            //перемещение пешехода
            _mySpriteRenderer.flipX = Controller.HorisontalAxis < 0;
            transform.position += (Vector3)(Velocity * Time.fixedDeltaTime);
        }

        /// <summary>
        /// Стрельба из турелей космического пешехода.
        /// </summary>
        public void Fire(TurretType turretType)
        {
            foreach (Turret turret in _turrets)
            {
                if (turret.Type != turretType) continue;
                turret.Fire();
            }
        }

        /*
        /// <summary>
        /// Добавление энергии кораблю.
        /// </summary>
        public void AddEnergy(int energy)
        {
            _primaryEnergy += energy;
            if (_primaryEnergy > _maxEnergy) _primaryEnergy = _maxEnergy;
        }

        /// <summary>
        /// Добавление патронов кораблю.
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
        /// Назначить новые свойства турелям пешехода.
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

        /// <summary>
        /// Применение настроек.
        /// </summary>
        public void Use(EnemyProperties properties)
        {
            _maxVelocity = properties.moveSpeed;
            base.Use(properties);
        }
    }
}
