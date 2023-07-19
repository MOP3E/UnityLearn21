using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class PlayerStatistics : IComparable
    {
        /// <summary>
        /// Имя игрока.
        /// </summary>
        public String PlayerName { get; set; }

        private int _kills;

        /// <summary>
        /// Количество убийств на уровне.
        /// </summary>
        public int Kills
        {
            get => _kills;
            set
            {
                _kills = value;
                KillsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Количество убийств на уровне изменилось.
        /// </summary>
        public event EventHandler KillsChanged;

        private int _score;

        /// <summary>
        /// Очки на уровне.
        /// </summary>
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                ScoreChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Количество очков на уровне изменилось.
        /// </summary>
        public event EventHandler ScoreChanged;

        private int _timeBounus;

        /// <summary>
        /// Дополнительные очки за время прохождения уровня (по 10% за каждый 1% сэкономленного времени).
        /// </summary>
        public int TimeBounus
        {
            get => _timeBounus;
            set
            {
                _timeBounus = value;
                TimeBounusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Количество дополнительных очков за время прохождения уровня изменилось.
        /// </summary>
        public event EventHandler TimeBounusChanged;

        private float _time;

        /// <summary>
        /// Время прохождения уровня.
        /// </summary>
        public float Time
        {
            get => _time;
            set
            {
                _time = value;
                TimeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Время уровня изменилось.
        /// </summary>
        public event EventHandler TimeChanged;

        /// <summary>
        /// Обнуление статистики.
        /// </summary>
        public void Reset()
        {
            Kills = 0;
            Score = 0;
            Time = 0;
        }

        public int CompareTo(object other)
        {
            if (other == null) return 1;
            PlayerStatistics statistics = (PlayerStatistics)other;

            if (_score.CompareTo(statistics._score) != 0) return _score.CompareTo(statistics._score);
            if (_time.CompareTo(statistics._time) != 0) return _time.CompareTo(statistics._score);
            return _kills.CompareTo(statistics._kills);
        }
    }
}
