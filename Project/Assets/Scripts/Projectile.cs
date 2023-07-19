using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    /// <summary>
    /// Управление перемещением снаряда, проверка на попадание в объекты.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// Идендификатор команды (урон наносится только членам других команд).
        /// </summary>
        [SerializeField] private int _teamId;

        /// <summary>
        /// Скорость снаряда.
        /// </summary>
        [SerializeField] private float _velocity;

        /// <summary>
        /// Скорость снаряда.
        /// </summary>
        public float Velocity => _velocity;

        /// <summary>
        /// Урон снаряда.
        /// </summary>
        [SerializeField] private int _damage;

        /// <summary>
        /// Префаб взрыва.
        /// </summary>
        [SerializeField] private GameObject _explodePrefab;

        /// <summary>
        /// Радиус жизни снаряда.
        /// </summary>
        [SerializeField] private float _lifeRadius;

        /// <summary>
        /// Радиус жизни снаряда.
        /// </summary>
        public float LifeRadius => _lifeRadius;

        /// <summary>
        /// Начальная позиция снаряда.
        /// </summary>
        private Vector2 _startPosition;

        /// <summary>
        /// Родительский корабль.
        /// </summary>
        public Destructible ParentDestructible { get; set; }

        /// <summary>
        /// Мой предок - игрок (для того, чтобы после смерти корабля игрока начислялись очки за попадание).
        /// </summary>
        private bool _myParentIsPlayer;


        private void Start()
        {
            _startPosition = transform.position;
            //для того, чтобы после смерти корабля игрока начислялись очки за попадание
            if (ParentDestructible != null) _teamId = ParentDestructible.TeamId;
            _myParentIsPlayer = Player.Instance != null && Player.Instance.ActiveWalker != null && ParentDestructible == Player.Instance.ActiveWalker;
        }

        /// <summary>
        /// FixedUpdate запускается с фиксированным периодом.
        /// </summary>
        private void FixedUpdate()
        {
            float stepLength = Time.deltaTime * _velocity;
            Vector2 step = transform.up * stepLength;

            //уничтожение снаряда при попадании
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);
            if (hit)
            {
                //Debug.Log("Projectile Hit!");

                Destructible destructible = hit.collider.transform.root.GetComponent<Destructible>();
                if (destructible != null && destructible != ParentDestructible && destructible.TeamId != _teamId)
                {
                    if (destructible.Hit(_damage) && _myParentIsPlayer)
                    {
                        Player.Instance.ScoreAdd(destructible.ScoreValue);
                        Player.Instance.KillAdd();
                    }

                    Debug.Log($"Уничтожено при попаданиии в: {destructible.nickname}");
                    OnProjectileDeath();
                    return;
                }
            }

            //уничтожение снаряда по прошествии заданного расстояния
            if (_lifeRadius <= (_startPosition - (Vector2)transform.position).magnitude)
            {
                Debug.Log($"Стартовая позиция: {_startPosition}");
                Debug.Log($"Уничтожено самоподрывом");
                OnProjectileDeath();
                return;
            }

            transform.position += new Vector3(step.x, step.y, 0);
        }

        /// <summary>
        /// Завершение жизни снаряда.
        /// </summary>
        private void OnProjectileDeath()
        {
            //создать эффект взрыва
            if (_explodePrefab != null)
            {
                ParticleSystem explode = Instantiate(_explodePrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                explode.Play();
            }

            Destroy(gameObject);
        }
    }
}
