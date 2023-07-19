using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Timer
    {
        /// <summary>
        /// Текущее время таймера.
        /// </summary>
        private float _time;

        /// <summary>
        /// Таймер завершил работу.
        /// </summary>
        public bool IsDone => _time <= 0;

        /// <summary>
        /// Создать и запустить таймер с заданным временем.
        /// </summary>
        public Timer(float time)
        {
            Start(time);
        }

        /// <summary> 
        /// Запустить таймер с заданным временем.
        /// </summary>
        public void Start(float time)
        {
            _time = time;
        }

        /// <summary> 
        /// Уменьшить время таймера на заданную величину.
        /// </summary>
        public void SubstractTime(float deltaTime)
        {
            if(_time <= 0) return;
            _time -= deltaTime;
        }
    }
}
