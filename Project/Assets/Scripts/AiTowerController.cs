using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Control;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace TowerDefense
{
    [RequireComponent(typeof(Tower))]
    public class AiTowerController : Controller
    {
        ///// <summary>
        ///// Тип поведения ИИ.
        ///// </summary>
        //[SerializeField] private AiTowerBehaviour _behaviour;

        /// <summary>
        /// Время между поиском новых целей.
        /// </summary>
        [SerializeField] private float _findNewTargetTime;

        /// <summary>
        /// Турель, которой управляет ИИ.
        /// </summary>
        private Tower _tower;

        /// <summary>
        /// Противник, который выбран в качестве цели.
        /// </summary>
        private Walker _selectedTarget;

        /// <summary>
        /// Таймер поиска новой цели.
        /// </summary>
        private Timer _findNewTargetTimer;

        //кэширование осей контроллера
        private AiAxis _primaryButton;
        private AiAxis _secondaryButton;

        internal AiTowerController() : base()
        {
            InitTimers();
        }

        private void Start()
        {
            //создать оси контпроллера
            PrimaryButton = gameObject.AddComponent<AiAxis>();
            _primaryButton = (AiAxis)PrimaryButton;
            SecondaryButton = gameObject.AddComponent<AiAxis>();
            _secondaryButton = (AiAxis)SecondaryButton;

            //закэшировать компоненты
            _tower = GetComponentInChildren<Tower>();

            //задать себя контроллером башни
            _tower.Controller = this;
        }

        private void Update()
        {
            UpdateTimers();
            UpdateAi();
        }

        private void InitTimers()
        {
            //_patrolZoneNewPointTimer = new Timer(_randomSelectMovePointTime);
            _findNewTargetTimer = new Timer(_findNewTargetTime);
        }

        private void UpdateTimers()
        {
            //таймер патрулирования зоны тикает только при патрулировании зоны
            //if (_behaviour == AiBehaviour.ZonePatrol) _patrolZoneNewPointTimer.SubstractTime(Time.deltaTime);
            _findNewTargetTimer.SubstractTime(Time.deltaTime);
        }

        private void UpdateAi()
        {
            //проверить расстояиеи до выбранной цели
            TestSelectedTarget();

            if (_selectedTarget == null)
            {
                //цель не выбрана - попытаться найти новую
                ActionFoundNewTarget();
            }
            else
            {
                //цель выбрана - стрелять
                ActionFire();
            }
        }

        /// <summary>
        /// Поиск новой цели.
        /// </summary>
        private void ActionFoundNewTarget()
        {
            //если цель уже выбрана либо таймер поиска цели не закончился - ничего не делать
            if (_selectedTarget != null || !_findNewTargetTimer.IsDone) return;

            //выбрать новую цель
            _selectedTarget = FindNearestWalker();

            //перезапустить таймер поиска цели
            _findNewTargetTimer.Start(_findNewTargetTime);
        }

        /// <summary>
        /// Поиск ближайшей к башни цели.
        /// </summary>
        private Walker FindNearestWalker()
        {
            //текущая дистанция до цели
            float currentDistance = float.MaxValue;
            //текущая выбранная цель
            Walker currentTarget = null;
            foreach (Destructible destructible in Destructible.AllDestructibles)
            {
                if(!(destructible is Walker walker)) continue;

                //пропускать неразрушимые, нейтральные и дружественные цели
                if (walker.IsIndestrictible ||
                    walker.TeamId == Destructible.NEUTRAL_TEAM_ID ||
                    walker.TeamId == _tower.TeamId) continue;

                //проверить дистанцию до цели
                float distance = Vector2.Distance(_tower.transform.position, walker.transform.position);
                if (distance > currentDistance) continue;
                
                //сохранить более близкую цель как текущую
                currentDistance = distance;
                currentTarget = walker;
                currentTarget.Destruction += OnSelectedTargetDestruction;
            }

            //вернуть более близкую цель, если она находится в радиусе поражения турели
            if (currentTarget == null) return null;
            return currentDistance < _tower.Turret.FiringRadius ? currentTarget : null;
        }

        /// <summary>
        /// Проверка, не вышла ли текущая цель за пределы зоны поражения.
        /// </summary>
        private void TestSelectedTarget()
        {
            if(_selectedTarget == null) return;
            float distance = Vector2.Distance(_tower.Turret.transform.position, _selectedTarget.transform.position);
            if (distance < _tower.Turret.FiringRadius) return;

            _selectedTarget.Destruction -= OnSelectedTargetDestruction;
            _selectedTarget = null;
        }

        /// <summary>
        /// Событие уничтожения выбранной цели.
        /// </summary>
        private void OnSelectedTargetDestruction(GameObject gameobject)
        {
            if(_selectedTarget != null) _selectedTarget.Destruction -= OnSelectedTargetDestruction;
            _selectedTarget = null;
        }

        private void ActionFire()
        {
            if (_selectedTarget == null) return;
            
            if (_tower.Turret.CanFire)
            {
                //рассчитать упреждение и выстрелить в заданную точку
                Vector2 target = MakeLead();
                Vector2 destination = target - (Vector2)_tower.Turret.transform.position;

                //определить угол, на который нужно повернуть турель перед выстрелом
                float alpha = 0f;
                if (destination.x == 0)
                {
                    if (destination.y < 0) alpha = 180f;
                }
                else if (destination.y == 0)
                {
                    alpha = destination.x > 0 ? 270f : 90f;
                }
                else if (destination.x > 0 && destination.y > 0)
                {
                    alpha = 270.0f + Mathf.Rad2Deg * Mathf.Acos(destination.x / destination.magnitude);
                }
                else if (destination.x > 0 && destination.y < 0)
                {
                    alpha = 270.0f - Mathf.Rad2Deg * Mathf.Acos(destination.x / destination.magnitude);
                }
                else if (destination.x < 0 && destination.y < 0)
                {
                    alpha = 90.0f + Mathf.Rad2Deg * Mathf.Acos(-destination.x / destination.magnitude);
                }
                else if (destination.x < 0 && destination.y > 0)
                {
                    alpha = 90.0f - Mathf.Rad2Deg * Mathf.Acos(-destination.x / destination.magnitude);
                }

                Rotation = Quaternion.Euler(new Vector3(0, 0, alpha));
                _primaryButton.AiValue = 1.0f;
            }
            else
            {
                _primaryButton.AiValue = 0.0f;
            }
        }

        public void SetBehaviourNone()
        {
            //_behaviour = AiTowerBehaviour.None;
        }

        /// <summary>
        /// Получить точку перехвата цели.
        /// </summary>
        private Vector2 MakeLead()
        {
            Vector2 shooterPosition = _tower.Turret.transform.position;
            Vector2 shooterVelocity = Vector2.zero; //у башни на башне нет скорости
            float shotSpeed = _tower.Turret.ProjectileVelocity;
            Vector2 targetPosition = _selectedTarget.transform.position;
            Vector2 targetVelocity = _selectedTarget.Velocity;

            return FirstOrderIntercept(shooterPosition, shooterVelocity, shotSpeed, targetPosition, targetVelocity);
        }

        /// <summary>
        /// Перехват первого порядка с использованием абсолютного положения цели
        /// </summary>
        /// <param name="shooterPosition">Позиция стрелка.</param>
        /// <param name="shooterVelocity">Скорость стрелка.</param>
        /// <param name="shotSpeed">Скорость выстрела.</param>
        /// <param name="targetPosition">Позиция цели.</param>
        /// <param name="targetVelocity">Скорость цели.</param>
        /// <returns></returns>
        private static Vector2 FirstOrderIntercept(Vector2 shooterPosition, Vector2 shooterVelocity, float shotSpeed, Vector2 targetPosition, Vector2 targetVelocity)
        {
            //рассчитать вектор скорости стрелка относительно цели
            Vector2 targetRelativeVelocity = targetVelocity - shooterVelocity;
            //рассчитать время, за которое может быть выполнен перехват
            float t = FirstOrderInterceptTime(shotSpeed, targetPosition - shooterPosition, targetRelativeVelocity);
            //получить позицию цели через заданное время
            return targetPosition + t * (targetRelativeVelocity);
        }

        /// <summary>
        /// Перехват первого порядка с использованием относительного положения цели.
        /// </summary>
        /// <param name="shotSpeed">Скорость выстрела.</param>
        /// <param name="targetRelativePosition">Позиция цели относительно стрелка.</param>
        /// <param name="targetRelativeVelocity">Скорость цели относительно стрелка.</param>
        /// <returns></returns>
        private static float FirstOrderInterceptTime(float shotSpeed, Vector2 targetRelativePosition, Vector2 targetRelativeVelocity)
        {
            //рассчитать квадрат относительной скорости
            float velocitySquared = targetRelativeVelocity.sqrMagnitude;
            if (velocitySquared < 0.001f) return 0f;

            //посчитать разницу квадратов относительной скорости цели и скорости выстрела
            float a = velocitySquared - shotSpeed * shotSpeed;

            //проверить, не совпадают ли скорости
            if (Mathf.Abs(a) < 0.001f)
            {
                float t = -targetRelativePosition.sqrMagnitude / (2f * Vector2.Dot(targetRelativeVelocity, targetRelativePosition));
                //don't shoot back in time
                return Mathf.Max(t, 0f);
            }

            float b = 2f * Vector2.Dot(targetRelativeVelocity, targetRelativePosition);
            float c = targetRelativePosition.sqrMagnitude;
            float determinant = b * b - 4f * a * c;

            //если детерминант > 0, есть два корня
            if (determinant > 0f)
            {
                //посчитать корни
                float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a);
                float t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);

                //вернуть наименьший из положительных корней, либо 0
                if (t1 > 0f && t2 > 0f) return Mathf.Min(t1, t2);
                if (t1 > 0f) return t1;
                if (t2 > 0f) return t2;
                return 0f;
            }

            //если детерминант < 0, корня не существует - вернуть 0
            if (determinant < 0.0) return 0f;

            //если детерминант = 0, есть только один корень
            //вернуть его если он больше нуля, в противном случае вернуть 0
            return Mathf.Max(-b / (2f * a), 0f);
        }
    }
}
