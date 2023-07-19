using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    /// <summary>
    /// Скрипт игрока.
    /// </summary>
    public class Player : MonoSingleton<Player>
    {
        /// <summary>
        /// Количество жизней.
        /// </summary>
        [Header("Player")]
        [SerializeField] private int _livesCount;

        /// <summary>
        /// Текущие жизни игрока.
        /// </summary>
        public int LivesCount
        {
            get => _livesCount;
            set
            {
                _livesCount = value;
                LivesCountChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Очки игрока изменились.
        /// </summary>
        public event EventHandler LivesCountChanged;

        /// <summary>
        /// Космический корабль на сцене.
        /// </summary>
        [SerializeField] private Walker _walker;

        /// <summary>
        /// Космический корабль игрока на сцене.
        ///  </summary>
        public Walker ActiveWalker => _walker;

        /// <summary>
        /// Префаб взрыва.
        /// </summary>
        [SerializeField] private GameObject _explodePrefab;

        ///// <summary>
        ///// Контроллер камеры.
        ///// </summary>
        //[SerializeField] private CameraContoller _cameraContoller;

        /// <summary>
        /// Контроллер управления движением.
        /// </summary>
        [SerializeField] private Controller _walkerController;

        private int _score;

        /// <summary>
        /// Очки игрока.
        /// </summary>
        public int Score
        {
            get => _score;
            private set
            {
                _score = value;
                ScoreChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Очки игрока изменились.
        /// </summary>
        public event EventHandler ScoreChanged;

        private int _kills;

        /// <summary>
        /// Количество убийств игрока.
        /// </summary>
        public int Kills
        {
            get => _kills;
            private set
            {
                _kills = value;
                KillsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Событие изменения количества убийств игрока.
        /// </summary>
        public event EventHandler KillsChanged;

        protected override void Awake()
        {
            base.Awake();

            if(_walker != null)
            {
                _walker.Destruction -= ShipDestructed;
                Destroy(_walker);
            }
        }

        private void Start()
        {
            Respawn();
        }

        /// <summary>
        /// Обработчик события разрушения корабля игрока.
        /// </summary>
        private void ShipDestructed(GameObject go)
        {
            Vector3 position = go.transform.position;
            LivesCount--;
            StartCoroutine(Respawn(position));
        }

        /// <summary>
        /// Взрыв старого и размещение нового корабля игрока на карте.
        /// </summary>
        private IEnumerator Respawn(Vector3 position)
        {
            //создать эффект взрыва корабля
            ParticleSystem explode = Instantiate(_explodePrefab, position, Quaternion.identity).GetComponent<ParticleSystem>();
            explode.Play();
            //ждать завершения работы эффекта взрыва
            yield return new WaitForSeconds(explode.main.duration);

            //разместить новый корабль игрока
            if(LivesCount > 0)
                Respawn();
            else
                LevelSequenceController.Instance.FinishCurrentLevel(false);
            yield return null;
        }

        /// <summary>
        /// Размещение нового корабля игрока на карте.
        /// </summary>
        private void Respawn()
        {
            if (LevelSequenceController.PlayerWalker != null)
            {
                //создать новый корабль игрока
                GameObject newWalker = Instantiate(LevelSequenceController.PlayerWalker.gameObject);
                //получить ссылку на корабль
                _walker = newWalker.GetComponent<Walker>();
                //настроить камеру
                //_cameraContoller.Tagret = newShip.GetComponentInChildren<CameraTarget>().transform;
                //настроить управление кораблём
                _walker.Controller = _walkerController;
                //подписаться на событие разрушения корабля
                _walker.Destruction += ShipDestructed;
            }
        }

        /// <summary>
        /// Увеличение счётчика очков.
        /// </summary>
        public void ScoreAdd(int score)
        {
            Score += score;
        }

        /// <summary>
        /// Увеличение счётчика убийств.
        /// </summary>
        public void KillAdd(int count = 1)
        {
            Kills += count;
        }
    }
}
