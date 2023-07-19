using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public interface ILevelCondition
    {
        bool IsCompleted { get; }
    }

    public class LevelController : MonoSingleton<LevelController>
    {
        /// <summary>
        /// Время, за которое должен быть пройден уровень, с.
        /// </summary>
        [SerializeField] private int _referenceTime;

        /// <summary>
        /// Это лабиринт - нет врагов, приз идёт только за время.
        /// </summary>
        [SerializeField] private bool _isMaze;

        /// <summary>
        /// Это лабиринт - нет врагов, приз идёт только за время.
        /// </summary>
        public bool IsMaze => _isMaze;

        /// <summary>
        /// Время, за которое должен быть пройден уровень, с.
        /// </summary>
        public int ReferenceTime => _referenceTime;
        
        /// <summary>
        /// Событие изменения количества убийств игрока.
        /// </summary>
        public event EventHandler LevelCompleted;

        /// <summary>
        /// Условия завершения уровня.
        /// </summary>
        private ILevelCondition[] _conditions;

        /// <summary>
        /// Уровень завершён.
        /// </summary>
        private bool _isCompleted;

        /// <summary>
        /// Бонусное время, оставшееся до конца уровня.
        /// </summary>
        private float _levelTime;

        /// <summary>
        /// Бонусное время, оставшееся до конца уровня.
        /// </summary>
        public float LevelTime
        {
            get => _levelTime;
            private set
            {
                _levelTime = value;
                LevelTimeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Событие изменения бонусного времени.
        /// </summary>
        public event EventHandler LevelTimeChanged;

        /// <summary>
        /// Start запускается перед первым кадром.
        /// </summary>
        private void Start()
        {
            _conditions = GetComponentsInChildren<ILevelCondition>();
            Debug.Log(_conditions.Length);
            LevelTime = ReferenceTime;
        }

        /// <summary>
        /// Update запускается каждый кадр.
        /// </summary>
        private void Update()
        {
            if (!_isCompleted)
            {
                if(LevelTime > 0) LevelTime -= Time.deltaTime;
                CheckLevelConditions();
            }
        }

        /// <summary>
        /// Проверка услвий завершения уровня.
        /// </summary>
        private void CheckLevelConditions()
        {
            if(_conditions == null || _conditions.Length == 0) return;

            foreach (ILevelCondition condition in _conditions)
            {
                if(!condition.IsCompleted) return;
            }

            _isCompleted = true;
            LevelCompleted?.Invoke(this, EventArgs.Empty);

            LevelSequenceController.Instance?.FinishCurrentLevel(true);
        }
    }
}
